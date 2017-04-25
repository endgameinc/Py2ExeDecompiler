using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace Py2ExeDecompiler
{
    public struct PythonScriptInfo
    {
        public int tag;
        public int optimize;
        public int unbuffered;
        public int data_bytes;
        public string zippath;
        public byte[] pycodeobject;
        

        public static string ReadNullTerminatedString(System.IO.BinaryReader stream)
        {
            string str = string.Empty;
            char ch;
            while ((int)(ch = stream.ReadChar()) != 0)
                str = str + ch;
            return str;
        }
        public static PythonScriptInfo FromArray(byte[] bytes)
        {
            var reader = new BinaryReader(new MemoryStream(bytes));

            var s = default(PythonScriptInfo);
            s.tag = reader.ReadInt32();
            s.optimize = reader.ReadInt32();
            s.unbuffered = reader.ReadInt32();
            s.data_bytes = reader.ReadInt32();
            s.zippath = ReadNullTerminatedString(reader);
            s.pycodeobject = reader.ReadBytes(s.data_bytes);
            return s;
        }
    }
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("Kernel32.dll", EntryPoint = "LockResource")]
        private static extern IntPtr LockResource(IntPtr hGlobal);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr FindResource(IntPtr hModule, int lpName, string lpType);
        //static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.formSkin2 = new FlatUI.FormSkin();
            this.flatButton3 = new FlatUI.FlatButton();
            this.flatButton2 = new FlatUI.FlatButton();
            this.flatLabel2 = new FlatUI.FlatLabel();
            this.flatToggle1 = new FlatUI.FlatToggle();
            this.flatClose1 = new FlatUI.FlatClose();
            this.flatMax1 = new FlatUI.FlatMax();
            this.flatLabel1 = new FlatUI.FlatLabel();
            this.flatButton1 = new FlatUI.FlatButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.formSkin2.SuspendLayout();
            this.SuspendLayout();
            // 
            // formSkin2
            // 
            this.formSkin2.BackColor = System.Drawing.Color.White;
            this.formSkin2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.formSkin2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(58)))), ((int)(((byte)(60)))));
            this.formSkin2.Controls.Add(this.flatButton3);
            this.formSkin2.Controls.Add(this.flatButton2);
            this.formSkin2.Controls.Add(this.flatLabel2);
            this.formSkin2.Controls.Add(this.flatToggle1);
            this.formSkin2.Controls.Add(this.flatClose1);
            this.formSkin2.Controls.Add(this.flatMax1);
            this.formSkin2.Controls.Add(this.flatLabel1);
            this.formSkin2.Controls.Add(this.flatButton1);
            this.formSkin2.Controls.Add(this.textBox1);
            this.formSkin2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formSkin2.FlatColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(66)))), ((int)(((byte)(224)))));
            this.formSkin2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.formSkin2.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.formSkin2.HeaderMaximize = false;
            this.formSkin2.Location = new System.Drawing.Point(0, 0);
            this.formSkin2.MinimumSize = new System.Drawing.Size(800, 500);
            this.formSkin2.Name = "formSkin2";
            this.formSkin2.Size = new System.Drawing.Size(800, 500);
            this.formSkin2.TabIndex = 0;
            this.formSkin2.Text = "Py2ExeDecompiler";
          
            // 
            // flatButton3
            // 
            this.flatButton3.BackColor = System.Drawing.Color.Transparent;
            this.flatButton3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(66)))), ((int)(((byte)(244)))));
            this.flatButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatButton3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.flatButton3.Location = new System.Drawing.Point(12, 142);
            this.flatButton3.Name = "flatButton3";
            this.flatButton3.Rounded = false;
            this.flatButton3.Size = new System.Drawing.Size(106, 32);
            this.flatButton3.TabIndex = 15;
            this.flatButton3.Text = "Clear";
            this.flatButton3.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton3.Click += new System.EventHandler(this.flatButton3_Click);
            // 
            // flatButton2
            // 
            this.flatButton2.BackColor = System.Drawing.Color.Transparent;
            this.flatButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(66)))), ((int)(((byte)(244)))));
            this.flatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatButton2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.flatButton2.Location = new System.Drawing.Point(12, 104);
            this.flatButton2.Name = "flatButton2";
            this.flatButton2.Rounded = false;
            this.flatButton2.Size = new System.Drawing.Size(106, 32);
            this.flatButton2.TabIndex = 14;
            this.flatButton2.Text = "Save";
            this.flatButton2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton2.Click += new System.EventHandler(this.flatButton2_Click);
            // 
            // flatLabel2
            // 
            this.flatLabel2.AutoSize = true;
            this.flatLabel2.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.flatLabel2.ForeColor = System.Drawing.Color.White;
            this.flatLabel2.Location = new System.Drawing.Point(8, 181);
            this.flatLabel2.Name = "flatLabel2";
            this.flatLabel2.Size = new System.Drawing.Size(88, 19);
            this.flatLabel2.TabIndex = 13;
            this.flatLabel2.Text = "DeObfuscate";
            // 
            // flatToggle1
            // 
            this.flatToggle1.BackColor = System.Drawing.Color.Transparent;
            this.flatToggle1.Checked = false;
            this.flatToggle1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatToggle1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.flatToggle1.Location = new System.Drawing.Point(12, 203);
            this.flatToggle1.Name = "flatToggle1";
            this.flatToggle1.Options = FlatUI.FlatToggle._Options.Style1;
            this.flatToggle1.Size = new System.Drawing.Size(76, 33);
            this.flatToggle1.TabIndex = 12;
            this.flatToggle1.Text = "flatToggle1";
            this.flatToggle1.CheckedChanged += new FlatUI.FlatToggle.CheckedChangedEventHandler(this.flatToggle1_CheckedChanged);
            // 
            // flatClose1
            // 
            this.flatClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flatClose1.BackColor = System.Drawing.Color.White;
            this.flatClose1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(66)))), ((int)(((byte)(244)))));
            this.flatClose1.Font = new System.Drawing.Font("Marlett", 10F);
            this.flatClose1.Location = new System.Drawing.Point(770, 12);
            this.flatClose1.Name = "flatClose1";
            this.flatClose1.Size = new System.Drawing.Size(18, 18);
            this.flatClose1.TabIndex = 10;
            this.flatClose1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // flatMax1
            // 
            this.flatMax1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flatMax1.BackColor = System.Drawing.Color.White;
            this.flatMax1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatMax1.Font = new System.Drawing.Font("Marlett", 12F);
            this.flatMax1.Location = new System.Drawing.Point(746, 12);
            this.flatMax1.Name = "flatMax1";
            this.flatMax1.Size = new System.Drawing.Size(18, 18);
            this.flatMax1.TabIndex = 11;
            this.flatMax1.Text = "flatMax1";
            this.flatMax1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // flatLabel1
            // 
            this.flatLabel1.AutoSize = true;
            this.flatLabel1.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.flatLabel1.ForeColor = System.Drawing.Color.White;
            this.flatLabel1.Location = new System.Drawing.Point(221, 22);
            this.flatLabel1.Name = "flatLabel1";
            this.flatLabel1.Size = new System.Drawing.Size(138, 19);
            this.flatLabel1.TabIndex = 10;
            this.flatLabel1.Text = "by @malwareunicorn";
            // 
            // flatButton1
            // 
            this.flatButton1.BackColor = System.Drawing.Color.Transparent;
            this.flatButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(66)))), ((int)(((byte)(244)))));
            this.flatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatButton1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.flatButton1.Location = new System.Drawing.Point(12, 65);
            this.flatButton1.Name = "flatButton1";
            this.flatButton1.Rounded = false;
            this.flatButton1.Size = new System.Drawing.Size(106, 32);
            this.flatButton1.TabIndex = 0;
            this.flatButton1.Text = "Upload";
            this.flatButton1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton1.Click += new System.EventHandler(this.uploadButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(135, 65);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(650, 425);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Drag and Drop or select Upload" + Environment.NewLine;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.formSkin2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Py2ExeDecompiler";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ClientSizeChanged += new System.EventHandler(this.flatMax1_Click);
            this.formSkin2.ResumeLayout(false);
            this.formSkin2.PerformLayout();
            this.ResumeLayout(false);
            this.Icon = Properties.Resources.Icon1;
            this.ShowIcon = true;

        }
        private void flatToggle1_CheckedChanged(object sender)
        {
            this.deobfuscate = this.flatToggle1.Checked;
            if (this.deobfuscate)
            {
                this.textBox1.Text += "DeObfuscation Enabled" + Environment.NewLine;
            }
            else
            {
                this.textBox1.Text += "DeObfuscation Disabled" + Environment.NewLine;
            }
        }
        private void flatButton2_Click(object sender, EventArgs e)
        {
            TextWriter fileToSave;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileToSave = new StreamWriter(saveFileDialog1.FileName);
                    fileToSave.WriteLine(textBox1.Text);
                    fileToSave.Close();
                }
            }
        }
        private void uploadButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = @"PE files | *.exe"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                processFile(dialog.FileName); // get name of file
            }
        }
        private void flatMax1_Click(object sender, EventArgs e)
        {
            switch (FindForm().WindowState)
            {
                case FormWindowState.Maximized:
                    this.textBox1.Size = new System.Drawing.Size(this.Width - 135, this.Height - 65);
                    break;
                case FormWindowState.Normal:
                    this.textBox1.Size = new System.Drawing.Size(this.Width - 135, this.Height - 65);
                    break;
            }
        }
        private void flatButton3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;

        }
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) processFile(file);
        }
        #endregion
        private byte[] extractResource(string input)
        {
            byte[] pycodeobject = null;
            PythonScriptInfo py_script_info;
            //Open File
            //Extract Resource as ByteArray
            IntPtr hMod = LoadLibrary(input);
            if (hMod == IntPtr.Zero)
            {
                textBox1.Text = @"LoadLibrary Failed";
                goto CleanUp;
            }

            IntPtr hRes = FindResource(hMod, 1, "PYTHONSCRIPT");
            if (hRes == IntPtr.Zero)
            {
                textBox1.Text = @"FindResource Failed";
                goto CleanUp;
            }
            uint size = SizeofResource(hMod, hRes);
            IntPtr pt = LoadResource(hMod, hRes);
            if (pt == IntPtr.Zero)
            {
                textBox1.Text = @"LoadResource Failed";
                goto CleanUp;
            }

            byte[] bPtr = new byte[size];
            IntPtr lockedpt = LockResource(pt);
            Marshal.Copy(lockedpt, bPtr, 0, (int)size);
            py_script_info = PythonScriptInfo.FromArray(bPtr);
            if (py_script_info.data_bytes > 0)
            {
                string zippath = py_script_info.zippath;
                pycodeobject = py_script_info.pycodeobject;
                textBox1.Text += String.Format(@"{2}PycodeObject Size: {1}{2}",
                                              zippath, py_script_info.data_bytes,
                                              Environment.NewLine);
            }

            CleanUp:
            return pycodeobject;
        }
        private string findPythonInstallPath(int version)
        {
            string exePath = null;
            string[] currentVersion = { };
            string[] version2 = { "2.5", "2.6", "2.7" };
            string[] version3 = { "3.0", "3.1", "3.2", "3.3", "3.4", "3.5", "3.5-32", "3.5-64", "3.6", "3.6-32", "3.6-64" };

            string userKey = "HKEY_CURRENT_USER\\";
            string pythonKeyName = "SOFTWARE\\Python\\PythonCore\\";
            string installPath = "\\InstallPath";
            string value = null;

            if (version == 2)
            {
                currentVersion = version2;
            }
            else if (version == 3)
            {
                currentVersion = version3;
            }

            for (int i = 0; i < currentVersion.Length; i++)
            {
                // Check HKEY_CURRENT_USER key
                string targetUserKey = userKey + pythonKeyName + currentVersion[i] + installPath;
                if (Registry.GetValue(targetUserKey, value, null) != null)
                {
                    //if key exists
                    exePath = (string)Registry.GetValue(targetUserKey, value, null);
                    goto CleanUp;
                }
                // Check HKEY_LOCAL_MACHINE Key
                string targetLocalKey = pythonKeyName + currentVersion[i] + installPath;

                // Check HKEY_LOCAL_MACHINE key 32 Bit
                RegistryKey localKey32 = RegistryKey.OpenBaseKey(
                    Microsoft.Win32.RegistryHive.LocalMachine,
                    RegistryView.Registry32);
                if (localKey32 == null)
                {
                    Debug.WriteLine(@"HKEY_LOCAL_MACHINE 32bit is null" + Environment.NewLine);
                    goto Check64BitKey;
                }
                localKey32 = localKey32.OpenSubKey(targetLocalKey, false);
                if (localKey32 == null)
                {
                    Debug.WriteLine(targetLocalKey + @" is null" + Environment.NewLine);
                    goto Check64BitKey;
                }
                if (localKey32.GetValue(null) != null)
                {
                    var keyValue = localKey32.GetValue(null);
                    if (keyValue == null)
                    {
                        Debug.WriteLine(@"Key is null" + Environment.NewLine);
                        goto Check64BitKey;
                    }
                    else
                    {
                        exePath = (string)keyValue;
                        goto CleanUp;
                    }
                    
                }
                
                Check64BitKey:
                // Check HKEY_LOCAL_MACHINE key 64 Bit
                RegistryKey localKey64 = RegistryKey.OpenBaseKey(
                    Microsoft.Win32.RegistryHive.LocalMachine,
                    RegistryView.Registry64);

                if (localKey64 == null)
                {
                    Debug.WriteLine(@"HKEY_LOCAL_MACHINE 64bit is null" + Environment.NewLine);
                    continue;
                }
                localKey64 = localKey64.OpenSubKey(targetLocalKey, false);
                if (localKey64 == null)
                {
                    Debug.WriteLine(targetLocalKey + @" is null" + Environment.NewLine);
                    continue;
                }
                if (localKey64.GetValue(null) != null)
                {
                    var keyValue = localKey64.GetValue(null);
                    if (keyValue == null)
                    {
                        Debug.WriteLine(@"Key is null" + Environment.NewLine);
                        continue;
                    }
                    else
                    {
                        exePath = (string)keyValue;
                        goto CleanUp;
                    }
                }
            }
            CleanUp:
            return exePath;
        }
        private string run_cmd(string filename, string cmd, string args)
        {
            string result = string.Empty;
            ProcessStartInfo start = new ProcessStartInfo();
            start.CreateNoWindow = true;
            start.FileName = filename;
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }

                using (StreamReader readerError = process.StandardError)
                {
                    this.textBox1.Text += readerError.ReadToEnd();
                }
            }
            return result;
        }
        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }
        private void processFile(string path)
        {
            string pycpyPath = null;
            string uncompylePath = Path.GetTempPath() + @"\uncompyle6.exe";
            int pythonVersion = 0;

            byte[] bPtr = null;
            if (path == null)
            {
                textBox1.Text += "ERROR: FileName is Null" + Environment.NewLine;
                goto CleanUp;
            }
            textBox1.Text = @"Uploaded File: " + path + Environment.NewLine;

            // Extract pycodeobject
            bPtr = extractResource(path);
            if (bPtr == null)
            {
                textBox1.Text += "ERROR: Py2Exe Resource is Null" + Environment.NewLine;
                goto CleanUp;
            }

            //Check the version of python
            if (bPtr[0] == 91)  // Python version 2 == 0x13
            {
                textBox1.Text += @"Python Version: 2" + Environment.NewLine;
                pythonVersion = 2;
            }
            else if (bPtr[0] == 219) // Python version 3 == 0xDB
            {
                textBox1.Text += @"Python Version: 3" + Environment.NewLine;
                pythonVersion = 3;
            }
            else
            {
                textBox1.Text += @"ERROR Python Version could not be determined" + Environment.NewLine;
                goto CleanUp;
            }

            // Get the python installation path
            string exePath = findPythonInstallPath(pythonVersion);
            if (exePath == null || exePath == string.Empty)
            {
                textBox1.Text += string.Format(@"ERROR: Python {0} is missing. Please Install {1}{2}",
                    pythonVersion,
                    exePath,
                    Environment.NewLine);
                goto CleanUp;
            }
            textBox1.Text += @"InstallPath: " + exePath + Environment.NewLine;

            // Save pyc and uncompyle6 to temp
            pycpyPath = string.Format(@"{0}\pyc{1}.py", Path.GetTempPath(), pythonVersion);
            if (!File.Exists(pycpyPath))
            {
                if (pythonVersion == 2)
                {
                    File.WriteAllBytes(pycpyPath, Properties.Resources.pyc2);
                }
                else if (pythonVersion == 3)
                {
                    File.WriteAllBytes(pycpyPath, Properties.Resources.pyc3);
                }
            }
            if (!File.Exists(uncompylePath))
            {
                File.WriteAllBytes(uncompylePath, Properties.Resources.uncompyle6);
            }

            //Run python pyc script
            string inputPycode = Convert.ToBase64String(bPtr);
            if (inputPycode == null)
            {
                textBox1.Text += @"ERROR: Unable to decode Pycode input" + Environment.NewLine;
                goto CleanUp;
            }
            string pythonpath = exePath + @"python.exe";
            string pythonOutput = string.Empty;
            if (this.deobfuscate)
            {
                pythonOutput = run_cmd(pythonpath, pycpyPath, inputPycode + " True");
            }
            else
            {
                pythonOutput = run_cmd(pythonpath, pycpyPath, inputPycode + " False");
            }
            if (pythonOutput == null || pythonOutput == string.Empty)
            {
                textBox1.Text += string.Format(@"ERROR: {0} failed{1}", pycpyPath, Environment.NewLine);
                goto CleanUp;
            }

            //save pyc files in temp
            var outputArray = pythonOutput.Split(new[] { '\r', '\n' });
            if (outputArray.Length < 1)
            {
                textBox1.Text += @"ERROR: No pyc file was recovered" + Environment.NewLine;
                goto CleanUp;
            }

            //Run Uncompyle6 on each pyc file
            for (int i = 0; i < outputArray.Length; i++)
            {
                var fileinfo = outputArray[i].Split(new[] { ';' });
                if (fileinfo.Length == 2)
                {
                    string savepycfile = Path.GetTempPath() + fileinfo[0];
                    textBox1.Text += @"---------------------------------" + Environment.NewLine;
                    textBox1.Text += @"Decompiling: " + savepycfile + Environment.NewLine;
                    textBox1.Text += @"---------------------------------" + Environment.NewLine;
                    if (ByteArrayToFile(savepycfile, Convert.FromBase64String(fileinfo[1])))
                    {
                        string UncompuleOutput = run_cmd(uncompylePath, savepycfile, "");
                        textBox1.Text += UncompuleOutput + Environment.NewLine;
                    }
                    else
                        goto CleanUp;
                }
            }
            CleanUp:
            if (File.Exists(pycpyPath))
            {
                File.Delete(pycpyPath);
            }
            if (File.Exists(uncompylePath))
            {
                File.Delete(uncompylePath);
            }
            return;
        }
        private FlatUI.FlatButton flatButton1;
        private FlatUI.FlatClose flatClose1;
        private TextBox textBox1;
        private FlatUI.FlatLabel flatLabel1;
        private FlatUI.FormSkin formSkin2;
        private FlatUI.FlatMax flatMax1;
        private FlatUI.FlatToggle flatToggle1;
        private FlatUI.FlatLabel flatLabel2;
        private FlatUI.FlatButton flatButton2;
        private FlatUI.FlatButton flatButton3;
    }
}

