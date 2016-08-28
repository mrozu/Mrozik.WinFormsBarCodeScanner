﻿namespace Mrozik.WinFormsBarCodeScanner
{
    partial class BarCodeScannerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.cameraViewer = new Emgu.CV.UI.ImageBox();
            this.switchCameraButton = new System.Windows.Forms.Button();
            this.loadingPictureBox = new System.Windows.Forms.PictureBox();
            this.recognizedBarCode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cameraViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraViewer
            // 
            this.cameraViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraViewer.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.cameraViewer.Location = new System.Drawing.Point(10, 54);
            this.cameraViewer.Name = "cameraViewer";
            this.cameraViewer.Size = new System.Drawing.Size(313, 447);
            this.cameraViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.cameraViewer.TabIndex = 2;
            this.cameraViewer.TabStop = false;
            // 
            // switchCameraButton
            // 
            this.switchCameraButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.switchCameraButton.Location = new System.Drawing.Point(10, 10);
            this.switchCameraButton.Name = "switchCameraButton";
            this.switchCameraButton.Size = new System.Drawing.Size(313, 44);
            this.switchCameraButton.TabIndex = 3;
            this.switchCameraButton.Text = "Switch camera";
            this.switchCameraButton.UseVisualStyleBackColor = true;
            this.switchCameraButton.Click += new System.EventHandler(this.switchCameraButton_Click);
            // 
            // loadingPictureBox
            // 
            this.loadingPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadingPictureBox.Image = global::Mrozik.WinFormsBarCodeScanner.Properties.Resources.loading;
            this.loadingPictureBox.InitialImage = null;
            this.loadingPictureBox.Location = new System.Drawing.Point(10, 54);
            this.loadingPictureBox.Name = "loadingPictureBox";
            this.loadingPictureBox.Size = new System.Drawing.Size(313, 419);
            this.loadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingPictureBox.TabIndex = 4;
            this.loadingPictureBox.TabStop = false;
            // 
            // recognizedBarCode
            // 
            this.recognizedBarCode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.recognizedBarCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.recognizedBarCode.Location = new System.Drawing.Point(10, 473);
            this.recognizedBarCode.Name = "recognizedBarCode";
            this.recognizedBarCode.Size = new System.Drawing.Size(313, 28);
            this.recognizedBarCode.TabIndex = 5;
            this.recognizedBarCode.Text = "(barcode)";
            this.recognizedBarCode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BarCodeScannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 511);
            this.Controls.Add(this.loadingPictureBox);
            this.Controls.Add(this.recognizedBarCode);
            this.Controls.Add(this.cameraViewer);
            this.Controls.Add(this.switchCameraButton);
            this.Name = "BarCodeScannerForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BarCodeScannerForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.cameraViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox cameraViewer;
        private System.Windows.Forms.Button switchCameraButton;
        private System.Windows.Forms.PictureBox loadingPictureBox;
        private System.Windows.Forms.Label recognizedBarCode;
    }
}

