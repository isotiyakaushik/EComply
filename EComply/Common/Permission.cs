using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EComply.Common
{
    public class Permission
    {
        private void GrantFolderPermission(string folderPath, string userName, FileSystemRights rights = FileSystemRights.FullControl)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

                // User ને rule add કરો
                FileSystemAccessRule rule = new FileSystemAccessRule(
                    userName,                                   // e.g. "DOMAIN\\Username" કે "MachineName\\Username"
                    rights,                                      // e.g. FileSystemRights.Modify
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow
                );

                dirSecurity.AddAccessRule(rule);
                dirInfo.SetAccessControl(dirSecurity);

                MessageBox.Show($"Permission granted to {userName} on '{folderPath}'.",
                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Administrator rights are required to grant permissions. Please run the application as Administrator.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GrantFilePermission(string filePath, string userName, FileSystemRights rights = FileSystemRights.FullControl)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();

                FileSystemAccessRule rule = new FileSystemAccessRule(
                    userName,                    // e.g. "DOMAIN\\Username" or "MachineName\\Username"
                    rights,                      // e.g. FileSystemRights.Modify
                    AccessControlType.Allow
                );

                fileSecurity.AddAccessRule(rule);
                fileInfo.SetAccessControl(fileSecurity);

                MessageBox.Show($"Permission granted to {userName} on '{filePath}'.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Administrator rights are required to grant permissions. Please run the application as Administrator.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
