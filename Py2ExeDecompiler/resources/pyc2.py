import marshal
import imp
import struct
import os
import sys
import base64
import new
import dis
from dis import opmap, opname


class Bytecode():
    '''
    Class to store individual instruction as a node in the graph
    '''
    def __init__(self, addr, buffer, prev=None, next=None, xrefs=[]):
        self.opcode = ord(buffer[0])
        self.addr = addr

        if self.opcode >= dis.HAVE_ARGUMENT:
            self.oparg = ord(buffer[1]) | (ord(buffer[2]) << 8)
        else:
            self.oparg = None

        self.prev = prev
        self.next = next
        self.xrefs = []
        self.target = None
        self.co_lnotab = None

    def len(self):
        '''
        Returns the length of the bytecode
        1 for no argument
        3 for argument
        '''
        if self.opcode < dis.HAVE_ARGUMENT:
            return 1
        else:
            return 3

    def disassemble(self):
        '''
        Return disassembly of bytecode
        '''
        rvalue = opname[self.opcode].ljust(20)
        if self.opcode >= dis.HAVE_ARGUMENT:
            rvalue += " %04x" % (self.oparg)
        return rvalue

    def hex(self):
        '''
        Return ASCII hex representation of bytecode
        '''
        rvalue = "%02x" % self.opcode
        if self.opcode >= dis.HAVE_ARGUMENT:
            rvalue += "%02x%02x" % \
                    (self.oparg & 0xff, (self.oparg >> 8) & 0xff)
        return rvalue

    def bin(self):
        '''
        Return bytecode string
        '''
        if self.opcode >= dis.HAVE_ARGUMENT:
            return struct.pack("<BH", self.opcode, self.oparg)
        else:
            return struct.pack("<B", self.opcode)

    def get_target_addr(self):
        '''
        Returns the target address for the current instruction based on the
        current address.
        '''
        rvalue = None
        if self.opcode in dis.hasjrel:
            rvalue = self.addr + self.oparg + self.len()
        if self.opcode in dis.hasjabs:
            rvalue = self.oparg

        return rvalue


class BytecodeGraph():
    def __init__(self, code, base=0):
        self.base = base
        self.code = code
        self.head = None
        self.parse_bytecode()
        self.apply_lineno()

    def add_node(self, parent, bc, lnotab=None):
        '''
        Adds an instruction node to the graph
        '''
        # setup pointers for new node
        bc.next = parent.next
        bc.prev = parent
        if lnotab is None:
            bc.co_lnotab = parent.co_lnotab
        else:
            bc.co_lnotab = lnotab

        if parent.next is not None:
            parent.next.prev = bc

        parent.next = bc

    def apply_labels(self, start=None):
        '''
        Find all JMP REL and ABS bytecode sequences and update the target
        within branch instruction and add xref to the destination.
        '''
        for current in self.nodes(start):
            current.xrefs = []
            current.target = None

        for current in self.nodes(start):
            label = -1
            if current.opcode >= dis.HAVE_ARGUMENT:
                if current.opcode in dis.hasjrel:
                    label = current.addr+current.oparg+current.len()
                elif current.opcode in dis.hasjabs:
                    label = current.oparg

                if label >= 0:
                    if current not in self.bytecodes[label].xrefs:
                        self.bytecodes[label].xrefs.append(current)
                    current.target = self.bytecodes[label]
            current = current.next
        return

    def apply_lineno(self):
        '''
        Parses the code object co_lnotab list and applies line numbers to
        bytecode. This is used to create a new co_lnotab list after modifying
        bytecode.
        '''
        byte_increments = [ord(c) for c in self.code.co_lnotab[0::2]]
        line_increments = [ord(c) for c in self.code.co_lnotab[1::2]]

        lineno = self.code.co_firstlineno
        addr = self.base
        linenos = []

        for byte_incr, line_incr in zip(byte_increments, line_increments):
            addr += byte_incr
            lineno += line_incr
            linenos.append((addr, lineno))

        if linenos == []:
            return
        current_addr, current_lineno = linenos.pop(0)
        if linenos == []:
            return
        current_addr, next_lineno = linenos.pop(0)
        for x in self.nodes():
            if x.addr >= current_addr:
                current_lineno = next_lineno
                if len(linenos) != 0:
                    current_addr, next_lineno = linenos.pop(0)
            x.co_lnotab = current_lineno

    def calc_lnotab(self):
        '''
        Creates a new co_lineno after modifying bytecode
        '''
        rvalue = ""

        prev_lineno = self.code.co_firstlineno
        prev_offset = self.head.addr

        for current in self.nodes():
            if current.co_lnotab == prev_lineno:
                continue

            new_offset = current.co_lnotab - prev_lineno
            new_offset = 0xff if new_offset > 0xff else new_offset

            rvalue += struct.pack("BB", current.addr - prev_offset,
                                  (current.co_lnotab - prev_lineno) & 0xff)

            prev_lineno = current.co_lnotab
            prev_offset = current.addr
        return rvalue

    def delete_node(self, node):
        '''
        Deletes a node from the graph, removing the instruction from the
        produced bytecode stream
        '''

        # For each instruction pointing to instruction to be delete,
        # move the pointer to the next instruction
        for x in node.xrefs:
            x.target = node.next

            if node.next is not None:
                node.next.xrefs.append(x)

        # Clean up the doubly linked list
        if node.prev is not None:
            node.prev.next = node.next
        if node.next is not None:
            node.next.prev = node.prev
        if node == self.head:
            self.head = node.next

        del self.bytecodes[node.addr]

    def disassemble(self, start=None, count=None):
        '''
        Simple disassembly routine for analyzing nodes in the graph
        '''

        rvalue = ""
        for x in self.nodes(start):
            rvalue += "[%04d] %04x %-6s %s\n" % \
                    (x.co_lnotab, x.addr, x.hex(), x.disassemble())
        return rvalue

    def get_code(self, start=None):
        '''
        Produce a new code object based on the graph
        '''
        self.refactor()

        # generate a new co_lineno
        new_co_lineno = self.calc_lnotab()

        # generate new bytecode stream
        new_co_code = ""
        for x in self.nodes(start):
            new_co_code += x.bin()

        # create a new code object with modified bytecode and updated line numbers
        # a new code object is necessary because co_code is readonly
        rvalue = new.code(self.code.co_argcount,
                          self.code.co_nlocals,
                          self.code.co_stacksize,
                          self.code.co_flags,
                          new_co_code,
                          self.code.co_consts,
                          self.code.co_names,
                          self.code.co_varnames,
                          self.code.co_filename,
                          self.code.co_name,
                          self.code.co_firstlineno,
                          new_co_lineno)

        return rvalue

    def nodes(self, start=None):
        '''
        Iterator for stepping through bytecodes in order
        '''
        if start is None:
            current = self.head
        else:
            current = start

        while current is not None:
            yield current
            current = current.next

        raise StopIteration

    def parse_bytecode(self):
        '''
        Parses the bytecode stream and creates an instruction graph
        '''

        self.bytecodes = {}
        prev = None
        offset = 0

        targets = []

        while offset < len(self.code.co_code):
            next = Bytecode(self.base + offset,
                            self.code.co_code[offset:offset+3],
                            prev)

            self.bytecodes[self.base + offset] = next
            offset += self.bytecodes[offset].len()

            if prev is not None:
                prev.next = next

            prev = next

            if next.get_target_addr() is not None:
                targets.append(next.get_target_addr())

        for x in targets:
            if x not in self.bytecodes:
                print "Nonlinear issue at offset: %08x" % x

        self.head = self.bytecodes[self.base]
        self.apply_labels()
        return

    def patch_opargs(self, start=None):
        '''
        Updates branch instructions to correct offsets after adding or
        deleting bytecode
        '''
        for current in self.nodes(start):
            # No argument, skip to next
            if current.opcode < dis.HAVE_ARGUMENT:
                continue

            # Patch relative offsets
            if current.opcode in dis.hasjrel:
                current.oparg = current.target.addr - \
                                    (current.addr+current.len())

            # Patch absolute offsets
            elif current.opcode in dis.hasjabs:
                current.oparg = current.target.addr

    def refactor(self):
        '''
        iterates through all bytecodes and determines correct offset
        position in code sequence after adding or removing bytecode
        '''

        offset = self.base
        new_bytecodes = {}

        for current in self.nodes():
            new_bytecodes[offset] = current
            current.addr = offset
            offset += current.len()
            current = current.next

        self.bytecodes = new_bytecodes
        self.patch_opargs()
        self.apply_labels()


def remove_obf(code):
    code = bytearray(code)
    i = 0
    while i < len(code):
        op = code[i]

        if code[i] == opmap['ROT_TWO'] and code[i+1] == opmap['ROT_TWO']:
            code[i] = opmap['NOP']
            code[i+1] = opmap['NOP']
        elif code[i] == opmap['ROT_THREE'] and code[i+1] == opmap['ROT_THREE'] and code[i+2] == opmap['ROT_THREE']:
            code[i] = opmap['NOP']
            code[i+1] = opmap['NOP']
            code[i+2] = opmap['NOP']
        elif code[i] == opmap['LOAD_CONST'] and code[i+3] == opmap['POP_TOP']:
            code[i] = opmap['NOP']
            code[i+1] = opmap['NOP']
            code[i+2] = opmap['NOP']
            code[i+3] = opmap['NOP']
        i += 1
        if op >= dis.HAVE_ARGUMENT:
            i += 2
    return "".join(chr(c) for c in code)


def clean_ROT_TWO(bcg, skip_xrefs=True):
    '''
    Replace two sequential ROT_TWO sequences with NOPS
    '''
    count = 0

    for current in bcg.nodes():
        if current.next is None:
            break

        if current.opcode == opmap['ROT_TWO'] and \
           current.next.opcode == opmap['ROT_TWO']:
            if current.next.xrefs != [] and skip_xrefs:
                continue
            else:
                current.opcode = opmap['NOP']
                current.next.opcode = opmap['NOP']
                count += 1
    return count


def clean_ROT_THREE(bcg, skip_xrefs=True):
    '''
    Replace three sequential ROT_THREE sequences with NOPS
    '''
    count = 0

    for current in bcg.nodes():
        if current.next is None or current.next.next is None:
            break

        if current.opcode == opmap['ROT_THREE'] and \
                current.next.opcode == opmap['ROT_THREE'] and \
                current.next.next.opcode == opmap['ROT_THREE']:

            if (current.next.xrefs != [] or current.next.next.xrefs != []) \
                    and skip_xrefs:
                        continue
            else:
                    current.opcode = opmap['NOP']
                    current.next.opcode = opmap['NOP']
                    current.next.next.opcode = opmap['NOP']
                    count += 1
    return count


def clean_LOAD_POP(bcg, skip_xrefs=True):
    '''
    Replace LOAD_CONST/POP_TOP sequences with NOPS
    '''
    count = 0

    for current in bcg.nodes():
        if current.next is None:
            break

        if current.opcode == opmap['LOAD_CONST'] and \
                current.next.opcode == opmap['POP_TOP']:

            if current.next.xrefs != [] and skip_xrefs:
                continue
            else:
                current.opcode = opmap['NOP']
                current.next.opcode = opmap['NOP']
                count += 1
    return count


def clean_NOPS(bcg):
    '''
    Remove NOP instrustions from bytecode
    '''
    count = 0

    for current in bcg.nodes():
        if current.opcode == opmap['NOP']:
            bcg.delete_node(current)
            count += 1

    return count


def clean(code, skip_xrefs=True):

    bcg = BytecodeGraph(code)

    rot_two = clean_ROT_TWO(bcg, skip_xrefs)
    rot_three = clean_ROT_THREE(bcg, skip_xrefs)
    load_pop = clean_LOAD_POP(bcg, skip_xrefs)
    nops = clean_NOPS(bcg)

    # return new code object if modifications were made
    if rot_two > 0 or rot_three > 0 or load_pop > 0 or nops > 0:
        return bcg.get_code()

    return None


def main(argv):
    pycodeobject = argv[1]
    deobfuscate = "False"
    if len(argv) > 2:
        deobfuscate = argv[2]
    if pycodeobject is None:
        sys.exit(1)
    bytesdecoded = bytes(base64.b64decode(pycodeobject))
    ob = marshal.loads(bytesdecoded)

    for i in range(0, len(ob)):
        with open(str(i)+'.pyc', 'wb') as fc:
            fc.write(imp.get_magic())
            fc.close()
        with open(str(i)+'.pyc', 'a') as fc:
            x = int(os.stat(str(i)+'.pyc').st_mtime)
            fc.write(chr(x & 0xff))
            fc.write(chr((x >> 8) & 0xff))
            fc.write(chr((x >> 16) & 0xff))
            fc.write(chr((x >> 24) & 0xff))
            fc.close()
        with open(str(i)+'.pyc', 'ab') as fc:
            code = clean(ob[i])
            if "False" in deobfuscate:
                marshal.dump(ob[i], fc)
            elif code is None:
                marshal.dump(ob[i], fc)
            else:
                marshal.dump(code, fc)
            fc.close()
        with open(str(i)+'.pyc', 'rb') as fc:
            print str(i)+'.pyc;'+base64.b64encode(fc.read())
            fc.close()
        os.remove(str(i)+'.pyc')
    return


if __name__ == "__main__":
    main(sys.argv)
