using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Video_Conference
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class VideoStreamServer : System.Windows.Forms.Form
	{
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private TextBox IP_textBox;
        private TextBox device_number_textBox;
        private Label label2;
        private Button button1;
        private Button button2;
        private System.Windows.Forms.Timer timer1; 
		
		#region WebCam API
		const short WM_CAP = 1024; 
		const int WM_CAP_DRIVER_CONNECT = WM_CAP + 10; 
		const int WM_CAP_DRIVER_DISCONNECT = WM_CAP + 11; 
		const int WM_CAP_EDIT_COPY = WM_CAP + 30; 
		const int WM_CAP_SET_PREVIEW = WM_CAP + 50; 
		const int WM_CAP_SET_PREVIEWRATE = WM_CAP + 52; 
		const int WM_CAP_SET_SCALE = WM_CAP + 53; 
		const int WS_CHILD = 1073741824; 
		const int WS_VISIBLE = 268435456; 
		const short SWP_NOMOVE = 2; 
		const short SWP_NOSIZE = 1; 
		const short SWP_NOZORDER = 4; 
		const short HWND_BOTTOM = 1; 
		int iDevice = 0;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox Clientip;
        int hHwnd; 
		[System.Runtime.InteropServices.DllImport("user32", EntryPoint="SendMessageA")] 
		static extern int SendMessage(int hwnd, int wMsg, int wParam,  [MarshalAs(UnmanagedType.AsAny)] 
			object lParam); 
		[System.Runtime.InteropServices.DllImport("user32", EntryPoint="SetWindowPos")] 
		static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags); 
		[System.Runtime.InteropServices.DllImport("user32")] 
		static extern bool DestroyWindow(int hndw); 
		[System.Runtime.InteropServices.DllImport("avicap32.dll")] 
		static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int x, int y, int nWidth, short nHeight, int hWndParent, int nID); 
		[System.Runtime.InteropServices.DllImport("avicap32.dll")] 
		static extern bool capGetDriverDescriptionA(short wDriver, string lpszName, int cbName, string lpszVer, int cbVer);
		private void OpenPreviewWindow() 
		{
			int iHeight = 259;
			int iWidth = 369;
			// 
			//  Open Preview window in picturebox
			// 
			hHwnd = capCreateCaptureWindowA(iDevice.ToString (), (WS_VISIBLE | WS_CHILD), 0, 0, 640, 480, picCapture.Handle.ToInt32(), 0);
			// 
			//  Connect to device
			// 
			if (SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, iDevice, 0) == 1) 
			{
				// 
				// Set the preview scale
				// 
				SendMessage(hHwnd, WM_CAP_SET_SCALE, 1, 0);
				// 
				// Set the preview rate in milliseconds
				// 
				SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0);
				// 
				// Start previewing the image from the camera
				// 
				SendMessage(hHwnd, WM_CAP_SET_PREVIEW, 1, 0);
				// 
				//  Resize window to fit in picturebox
				// 
				SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, iWidth,iHeight, (SWP_NOMOVE | SWP_NOZORDER));
			}
			else 
			{
				// 
				//  Error connecting to device close window
				//  
				DestroyWindow(hHwnd);
			}
		}
		private void ClosePreviewWindow() 
		{
			// 
			//  Disconnect from device
			// 
			SendMessage(hHwnd, WM_CAP_DRIVER_DISCONNECT, iDevice, 0);
			// 
			//  close window
			// 
			DestroyWindow(hHwnd);
		}
		#endregion

        internal System.Windows.Forms.Button btnStop;
		internal System.Windows.Forms.Button btnStart;
        internal System.Windows.Forms.PictureBox picCapture;
        private IContainer components;

		public VideoStreamServer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.IP_textBox = new System.Windows.Forms.TextBox();
            this.device_number_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.picCapture = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Clientip = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Location = new System.Drawing.Point(283, 468);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 32);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "Stop Capturing";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(186, 468);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(91, 32);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start Capturing";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // IP_textBox
            // 
            this.IP_textBox.Location = new System.Drawing.Point(590, 412);
            this.IP_textBox.Name = "IP_textBox";
            this.IP_textBox.Size = new System.Drawing.Size(161, 20);
            this.IP_textBox.TabIndex = 13;
            this.IP_textBox.Text = "127.0.0.1";
            this.IP_textBox.Visible = false;
            // 
            // device_number_textBox
            // 
            this.device_number_textBox.Location = new System.Drawing.Point(284, 398);
            this.device_number_textBox.MaxLength = 2;
            this.device_number_textBox.Name = "device_number_textBox";
            this.device_number_textBox.Size = new System.Drawing.Size(48, 20);
            this.device_number_textBox.TabIndex = 15;
            this.device_number_textBox.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 401);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Capture Device #";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(590, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Listener";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(186, 516);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 29);
            this.button2.TabIndex = 18;
            this.button2.Text = "Start Sending";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // picCapture
            // 
            this.picCapture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picCapture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCapture.Image = global::Video_Conference.Properties.Resources.icony;
            this.picCapture.Location = new System.Drawing.Point(186, 88);
            this.picCapture.Name = "picCapture";
            this.picCapture.Size = new System.Drawing.Size(369, 259);
            this.picCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCapture.TabIndex = 6;
            this.picCapture.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(239, 36);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(248, 25);
            this.label3.TabIndex = 19;
            this.label3.Text = "VIDEO CHAT SERVER";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "AllowClientIP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(186, 437);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "StartPreview";
            // 
            // Clientip
            // 
            this.Clientip.Location = new System.Drawing.Point(267, 366);
            this.Clientip.MaxLength = 14;
            this.Clientip.Name = "Clientip";
            this.Clientip.Size = new System.Drawing.Size(100, 20);
            this.Clientip.TabIndex = 20;
            // 
            // VideoStreamServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 557);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Clientip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.device_number_textBox);
            this.Controls.Add(this.IP_textBox);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picCapture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VideoStreamServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Video Conference - Peer 2 - www.fadidotnet.org";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new VideoStreamServer());
		}
        TcpClient myclient;
        MemoryStream ms;
        NetworkStream myns;
        BinaryWriter mysw;
        Thread myth;
        TcpListener mytcpl;
        Socket mysocket;
        NetworkStream ns;

      
        
        private void Start_Sending_Video_Conference(string remote_IP, int port_number)
        {
            try
            {

                ms = new MemoryStream();// Store it in Binary Array as Stream


                IDataObject data;
                Image bmap;

                //  Copy image to clipboard
                SendMessage(hHwnd, WM_CAP_EDIT_COPY, 0, 0);

                //  Get image from clipboard and convert it to a bitmap
                data = Clipboard.GetDataObject();

                if (data.GetDataPresent(typeof(System.Drawing.Bitmap)))
                {
                    bmap = ((Image)(data.GetData(typeof(System.Drawing.Bitmap))));
                    bmap.Save(ms, ImageFormat.Bmp);
                }

                
                picCapture.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();
                myclient = new TcpClient(remote_IP, 5000);//Connecting with server
                myns = myclient.GetStream();
                mysw = new BinaryWriter(myns);
                mysw.Write(arrImage);//send the stream to above address
                ms.Flush();
                mysw.Flush();
                myns.Flush();
                ms.Close();
                mysw.Close();
                myns.Close();
                myclient.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Video Conference Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		private void btnStart_Click(object sender, System.EventArgs e)
		{
            try
            {
                iDevice = int.Parse(device_number_textBox.Text);
                OpenPreviewWindow();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Video Conference Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
		private void btnStop_Click(object sender, System.EventArgs e)
		{
            try
            {
                ClosePreviewWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Video Conference Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            try
            {
                if (btnStop.Enabled)
                {
                    ClosePreviewWindow();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Video Conference Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        

		}

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false;
            //myth = new Thread(new System.Threading.ThreadStart(Start_Receiving_Video_Conference)); // Start Thread Session
            //myth.Start(); // Start Receiveing Camera
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Start_Sending_Video_Conference(Clientip.Text,5000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mytcpl.Stop();
                myth.Abort();
            }
            catch (Exception ex){

                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        //private void picCapture_Click(object sender, EventArgs e)
        //{

        //}

        //private void label2_Click(object sender, EventArgs e)
        //{

        //}

        //private void device_number_textBox_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void IP_textBox_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void label1_Click(object sender, EventArgs e)
        //{

        //}
    }
}
