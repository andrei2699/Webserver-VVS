using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Webserver;
using Webserver.Config;
using Webserver.HTTPHeaders;
using Webserver.IO;
using Webserver.Request;
using Webserver.Response;

namespace WebserverGUI
{
     public partial class Form1 : Form
     {
          public Form1()
          {
               InitializeComponent();
          }

          private ServerConfigManager _serverConfigManager;
          private FileReader _fileReader;
          private ServerEvents _serverEvents;
          private ServerState _currentState;

          private void Form1_Load(object sender, EventArgs e)
          {
               _fileReader = new FileReader();
               _serverConfigManager = new ServerConfigManager(_fileReader, new FileWriter());

               var serverConfig = _serverConfigManager.ReadConfig();

               serverPortLabel.Text = serverConfig.Port.ToString();
               serverPortLabel.Text = serverConfig.Port.ToString();
               portNumericUpDown.Value = serverConfig.Port;
               rootPathTextBox.Text = serverConfig.FilePath;
               maintenanceFilePathTextBox.Text = serverConfig.MaintenanceFilePath;
          }

          private void startServerButton_Click(object sender, EventArgs e)
          {
               startServerButton.Enabled = false;
               stopServerButton.Enabled = true;
               switchToMaintenanceCheckBox.Enabled = true;
               switchToMaintenanceCheckBox.Checked = false;
               portNumericUpDown.Enabled = false;

               var filePathValidator = new FilePathValidator();
               var filePathProvider = new FilePathProvider(filePathValidator);

               _serverEvents = new ServerEvents();

               var server = new Server(
                    filePathProvider,
                    _serverConfigManager,
                    new RequestParser(),
                    new ResponseCreator(new ResponseHeaderParser(), filePathProvider, _fileReader,
                         new ContentTypeHeaderProvider(), new ConfigRequestHandler(_serverEvents)),
                    filePathValidator);

               _serverEvents.OnStatusChanged += state =>
               {
                    server.OnStatusChanged(state);
                    UpdateServerState(state);
               };
               _serverEvents.OnFilePathChanged += server.OnFilePathChanged;

               UpdateServerState(ServerState.Running);

               new Thread(() => { server.Start(); }).Start();
          }

          private void stopServerButton_Click(object sender, EventArgs e)
          {
               startServerButton.Enabled = true;
               stopServerButton.Enabled = false;
               switchToMaintenanceCheckBox.Enabled = false;
               switchToMaintenanceCheckBox.Checked = false;
               portNumericUpDown.Enabled = true;

               _serverEvents?.ChangeState(ServerState.Stopped);
          }

          private void switchToMaintenanceCheckBox_CheckedChanged(object sender, EventArgs e)
          {
               var state = switchToMaintenanceCheckBox.Checked
                    ? ServerState.Maintenance
                    : ServerState.Running;

               _serverEvents?.ChangeState(state);
          }

          private void openRootPathButton_Click(object sender, EventArgs e)
          {
               folderBrowserDialog1.SelectedPath = Path.GetFullPath(rootPathTextBox.Text);

               if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
               {
                    rootPathTextBox.Text = folderBrowserDialog1.SelectedPath;
               }
          }

          private void openMaintenanceFilePathButton_Click(object sender, EventArgs e)
          {
               openFileDialog1.InitialDirectory = Path.GetFullPath(maintenanceFilePathTextBox.Text);

               if (openFileDialog1.ShowDialog() == DialogResult.OK)
               {
                    maintenanceFilePathTextBox.Text = openFileDialog1.FileName;
               }
          }

          private void applyChangesButton_Click(object sender, EventArgs e)
          {
               var port = (int) portNumericUpDown.Value;

               _serverConfigManager.WriteConfig(new ServerConfig(port, rootPathTextBox.Text,
                    maintenanceFilePathTextBox.Text, _currentState));


               _serverEvents?.ChangeFilePath(_currentState == ServerState.Maintenance
                    ? maintenanceFilePathTextBox.Text
                    : rootPathTextBox.Text);

               serverPortLabel.Text = port.ToString();
          }

          private void Form1_FormClosing(object sender, FormClosingEventArgs e)
          {
               stopServerButton_Click(sender, e);
          }

          private void UpdateServerState(ServerState state)
          {
               _currentState = state;
               serverStateLabel.Text = state.ToString();


               serverPortLabel.Text = portNumericUpDown.Value.ToString();
          }
     }
}
