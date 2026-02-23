using System;
using System.IO;
using System.Windows.Forms;

namespace flashkit_md
{
    public partial class MainForm : Form
    {
        private const int BUFFER_SIZE = 65536; // 64 KB буфер для чтения

        private Button btnDump;
        private Button btnWrite;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblConnection;
        private Label lblSizeLabel;
        private Label lblTimer;
        private ComboBox cmbSize;
        private CheckBox chkByteSwap;
        private CheckBox chkVerify;
        private System.Windows.Forms.Timer timer;
        private int elapsedSeconds;
        private System.ComponentModel.IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private static readonly int[] DUMP_SIZES_MB = { 1, 2, 4, 8, 16, 32, 64, 128 };

        public MainForm()
        {
            InitializeComponent();
            CheckConnection();
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblConnection = new Label();
            lblSizeLabel = new Label();
            cmbSize = new ComboBox();
            lblTimer = new Label();
            chkByteSwap = new CheckBox();
            chkVerify = new CheckBox();
            btnDump = new Button();
            btnWrite = new Button();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            timer = new System.Windows.Forms.Timer(components);
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // lblConnection
            // 
            lblConnection.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblConnection.Location = new Point(20, 10);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(210, 28);
            lblConnection.TabIndex = 0;
            lblConnection.Text = "Соединение...";
            lblConnection.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSizeLabel
            // 
            lblSizeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSizeLabel.Location = new Point(23, 17);
            lblSizeLabel.Name = "lblSizeLabel";
            lblSizeLabel.Size = new Size(48, 24);
            lblSizeLabel.TabIndex = 1;
            lblSizeLabel.Text = "Размер";
            lblSizeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbSize
            // 
            cmbSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSize.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbSize.Items.AddRange(new object[] { "1 MB", "2 MB", "4 MB", "8 MB", "16 MB", "32 MB", "64 MB", "128 MB" });
            cmbSize.SelectedIndex = 1; // По умолчанию 2 MB
            cmbSize.Location = new Point(80, 19);
            cmbSize.Name = "cmbSize";
            cmbSize.Size = new Size(64, 23);
            cmbSize.TabIndex = 2;
            // 
            // lblTimer
            // 
            lblTimer.Location = new Point(0, 0);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(100, 23);
            lblTimer.TabIndex = 0;
            // 
            // chkByteSwap
            // 
            chkByteSwap.Checked = false;
			chkByteSwap.CheckState = CheckState.Unchecked;
			chkByteSwap.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            chkByteSwap.Location = new Point(300, 12);
            chkByteSwap.Name = "chkByteSwap";
            chkByteSwap.Size = new Size(80, 28);
            chkByteSwap.TabIndex = 3;
            chkByteSwap.Text = "Byte Swap";
			chkByteSwap.CheckedChanged += chkVerify_CheckedChanged;
            // 
            // chkVerify
            // 
            chkVerify.Checked = true;
            chkVerify.CheckState = CheckState.Checked;
            chkVerify.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            chkVerify.Location = new Point(26, 17);
            chkVerify.Margin = new Padding(0);
            chkVerify.Name = "chkVerify";
            chkVerify.Size = new Size(88, 28);
            chkVerify.TabIndex = 4;
            chkVerify.Text = "Проверить";
            chkVerify.CheckedChanged += chkVerify_CheckedChanged;
            // 
            // btnDump
            // 
            btnDump.Enabled = false;
            btnDump.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnDump.Location = new Point(25, 50);
            btnDump.Name = "btnDump";
            btnDump.Size = new Size(120, 38);
            btnDump.TabIndex = 5;
            btnDump.Text = "Dump";
            btnDump.Click += BtnDump_Click;
            // 
            // btnWrite
            // 
            btnWrite.Enabled = false;
            btnWrite.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnWrite.Location = new Point(25, 50);
            btnWrite.Name = "btnWrite";
            btnWrite.Size = new Size(120, 38);
            btnWrite.TabIndex = 6;
            btnWrite.Text = "Write";
            btnWrite.Click += BtnWrite_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(20, 150);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(360, 18);
            progressBar.TabIndex = 7;
            // 
            // lblStatus
            // 
            lblStatus.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.Location = new Point(20, 180);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(360, 60);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "Готов";
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblSizeLabel);
            groupBox1.Controls.Add(cmbSize);
            groupBox1.Controls.Add(btnDump);
            groupBox1.Location = new Point(20, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(170, 100);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkVerify);
            groupBox2.Controls.Add(btnWrite);
            groupBox2.Location = new Point(210, 40);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(170, 100);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            // 
            // MainForm
            // 
            ClientSize = new Size(400, 250);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblConnection);
            Controls.Add(chkByteSwap);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Flashkit Dumper";
            FormClosing += MainForm_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedSeconds++;
            lblTimer.Text = $"Время: {elapsedSeconds} сек";
        }

        private int GetSelectedSizeBytes()
        {
            return DUMP_SIZES_MB[cmbSize.SelectedIndex] * 1024 * 1024;
        }

        private void CheckConnection()
        {
            try
            {
                Device.connect();
                lblConnection.Text = $"Подключен {Device.getPortName()}";
                lblConnection.ForeColor = System.Drawing.Color.Green;
                btnDump.Enabled = true;
                btnWrite.Enabled = true;
                lblStatus.Text = "Нажмите Dump для чтения или Write для записи.";
            }
            catch (Exception ex)
            {
                lblConnection.Text = "Устройство не обнаружено";
                lblConnection.ForeColor = System.Drawing.Color.Red;
                btnDump.Enabled = false;
                lblStatus.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnDump_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                saveDialog.FileName = "Dump.bin";
                saveDialog.Title = "Сохранить дамп";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    DumpFlash(saveDialog.FileName);
                }
            }
        }

        private void DumpFlash(string filename)
        {
            int flashSize = GetSelectedSizeBytes();
            int flashSizeMb = flashSize / 1024 / 1024;

            btnDump.Enabled = false;
            btnWrite.Enabled = false;
            cmbSize.Enabled = false;
            chkByteSwap.Enabled = false;
            chkVerify.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "Чтение Flash...";

            // Запускаем таймер
            elapsedSeconds = 0;
            timer.Start();

            Application.DoEvents();

            try
            {
                byte[] buffer = new byte[flashSize];
                int bytesRead = 0;

                Device.setDelay(0);
                Device.setAddr(0);

                while (bytesRead < flashSize)
                {
                    int chunkSize = Math.Min(BUFFER_SIZE, flashSize - bytesRead);
                    Device.read(buffer, bytesRead, chunkSize);
                    bytesRead += chunkSize;

                    int progress = (int)((long)bytesRead * 100 / flashSize);
                    progressBar.Value = progress;
                    lblStatus.Text = $"Прочитано: {bytesRead / 1024} KB / {flashSize / 1024} KB ({progress}%)";
                    Application.DoEvents();
                }

                lblStatus.Text = "Сохранение файла...";
                Application.DoEvents();

                if (chkByteSwap.Checked == false)
                    SwapBytes(buffer);

                File.WriteAllBytes(filename, buffer);

                timer.Stop();
                progressBar.Value = 100;
                lblStatus.Text = $"ОК, файл сохранён: {filename} ({flashSizeMb} MB, {elapsedSeconds} сек)";
            }
            catch (Exception ex)
            {
                timer.Stop();
                progressBar.Value = 0;
                lblStatus.Text = $"Ошибка: {ex.Message}";
                MessageBox.Show(
                    $"Ошибка при чтении Flash памяти:\n\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnDump.Enabled = true;
                btnWrite.Enabled = true;
                cmbSize.Enabled = true;
                chkByteSwap.Enabled = true;
                chkVerify.Enabled = true;
            }
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                openDialog.Title = "Выбрать файл для записи";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    long fileSize = new FileInfo(openDialog.FileName).Length;
                    if (fileSize == 0 || fileSize % 2 != 0)
                    {
                        MessageBox.Show(
                            "Размер файла должен быть чётным и не равным нулю.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    DialogResult confirm = MessageBox.Show(
                        $"Будет выполнена запись файла:\n{openDialog.FileName}\n\nРазмер: {fileSize / 1024} KB\n\n" +
                        "Содержимое Flash будет полностью стёрто!\nПродолжить?",
                        "Подтверждение записи",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm == DialogResult.Yes)
                        WriteFlash(openDialog.FileName);
                }
            }
        }

        private void WriteFlash(string filename)
        {
            byte[] buffer;

            try
            {
                buffer = File.ReadAllBytes(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось прочитать файл:\n\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int fileSize = buffer.Length;

            // Если byte swap включён — применяем к данным перед записью
            if (chkByteSwap.Checked == false)
                SwapBytes(buffer);

            btnDump.Enabled = false;
            btnWrite.Enabled = false;
            cmbSize.Enabled = false;
            chkByteSwap.Enabled = false;
            chkVerify.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "Подготовка к записи...";

            elapsedSeconds = 0;
            timer.Start();

            Application.DoEvents();

            try
            {
                const int SECTOR_SIZE = 8 * 1024;       // 8 KB — один сектор
                const int ERASE_BLOCK = 8 * SECTOR_SIZE; // 64 KB — один вызов flashErase стирает 8 секторов
                const int WRITE_CHUNK = 512;             // байт за один вызов flashWrite

                int erasedBytes = 0;

                // ── Шаг 1: стираем секторы ──────────────────────────────────
                Device.setDelay(1);
                Device.flashUnlockBypass();

                for (int addr = 0; addr < fileSize; addr += ERASE_BLOCK)
                {
                    lblStatus.Text = $"Стирание: {erasedBytes / 1024} KB / {fileSize / 1024} KB...";
                    int progress = (int)((long)erasedBytes * 100 / fileSize); // прогресс стирания
                    progressBar.Value = progress;
                    Application.DoEvents();

                    Device.flashErase(addr);

                    erasedBytes += ERASE_BLOCK;
                    if (erasedBytes > fileSize) erasedBytes = fileSize;
                }

                // ── Шаг 2: записываем данные ────────────────────────────────
                Device.setAddr(0);
                int written = 0;


                while (written < fileSize)
                {
                    int chunkSize = Math.Min(WRITE_CHUNK, fileSize - written);
                    Device.flashWrite(buffer, written, chunkSize);
                    written += chunkSize;

                    int progress = (int)((long)written * 100 / fileSize); // прогресс записи
                    progressBar.Value = progress;
                    lblStatus.Text = $"Запись: {written / 1024} KB / {fileSize / 1024} KB ({progress}%)";
                    Application.DoEvents();
                }

                Device.flashResetByPass();

                // ── Шаг 3: верификация ─────────────────────────────────────
                if (chkVerify.Checked)
                {
                    lblStatus.Text = "Проверка данных...";
                    progressBar.Value = 0;
                    Application.DoEvents();

                    Device.setDelay(1);
                    Device.setAddr(0);

                    byte[] verifyBuffer = new byte[fileSize];
                    int verified = 0;

                    while (verified < fileSize)
                    {
                        int chunkSize = Math.Min(BUFFER_SIZE, fileSize - verified);
                        Device.read(verifyBuffer, verified, chunkSize);
                        verified += chunkSize;

                        int progress = (int)((long)verified * 100 / fileSize); // прогресс верификации
                        progressBar.Value = progress;
                        lblStatus.Text = $"Проверка: {verified / 1024} KB / {fileSize / 1024} KB ({progress}%)";
                        Application.DoEvents();
                    }

                    for (int i = 0; i < fileSize; i++)
                    {
                        if (verifyBuffer[i] != buffer[i])
                        {
                            throw new Exception(
                                $"Ошибка при проверке!\nАдрес: 0x{i:X6}\nFlash: 0x{verifyBuffer[i]:X2}\nФайл: 0x{buffer[i]:X2}"
                            );
                        }
                    }
                }

                timer.Stop();
                progressBar.Value = 100;
                lblStatus.Text = $"Запись завершена: {fileSize / 1024} KB за {elapsedSeconds} сек";
            }
            catch (Exception ex)
            {
                timer.Stop();
                Device.flashResetByPass();
                progressBar.Value = 0;
                lblStatus.Text = $"Ошибка записи: {ex.Message}";
                MessageBox.Show(
                    $"Ошибка при записи flash:\n\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnDump.Enabled = true;
                btnWrite.Enabled = true;
                cmbSize.Enabled = true;
                chkByteSwap.Enabled = true;
                chkVerify.Enabled = true;
            }
        }

        private void SwapBytes(byte[] buffer)
        {
            for (int i = 0; i + 1 < buffer.Length; i += 2)
            {
                byte tmp = buffer[i];
                buffer[i] = buffer[i + 1];
                buffer[i + 1] = tmp;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            try { Device.disconnect(); }
            catch { }
        }

        private void chkVerify_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
