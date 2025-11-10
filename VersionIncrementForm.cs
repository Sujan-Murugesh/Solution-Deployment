using Sujan_Solution_Deployer.Models;
using Sujan_Solution_Deployer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static Sujan_Solution_Deployer.Services.SolutionService;

namespace Sujan_Solution_Deployer
{
    public partial class VersionIncrementForm : Form
    {
        public Dictionary<string, string> UpdatedVersions { get; private set; }
        public bool UpdateInSource { get; private set; }
        private List<SolutionInfo> solutions;
        private SolutionService solutionService;
        private bool isUpdatingCell = false;
        private bool isUserEdit = false;
        public VersionIncrementForm(List<SolutionInfo> solutionsList, SolutionService service)
        {
            InitializeComponent();
            solutions = solutionsList;
            solutionService = service;
            UpdatedVersions = new Dictionary<string, string>();

            // ✅ Track when user starts editing
            dgvSolutions.CellBeginEdit += dgvSolutions_CellBeginEdit;
            dgvSolutions.CellEndEdit += dgvSolutions_CellEndEdit;

            LoadSolutions();
            //TestVersionIncrement();
        }

        // Add this method temporarily for testing
        private void dgvSolutions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            isUserEdit = true;
        }

        private void dgvSolutions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            isUserEdit = false;
        }
        private void TestVersionIncrement()
        {
            string testVersion = "1.0.0.1";

            MessageBox.Show(
                $"Original: {testVersion}\n\n" +
                $"Major: {solutionService.IncrementVersion(testVersion, SolutionService.VersionIncrementType.Major)}\n" +
                $"Minor: {solutionService.IncrementVersion(testVersion, SolutionService.VersionIncrementType.Minor)}\n" +
                $"Build: {solutionService.IncrementVersion(testVersion, SolutionService.VersionIncrementType.Build)}\n" +
                $"Revision: {solutionService.IncrementVersion(testVersion, SolutionService.VersionIncrementType.Revision)}",
                "Version Increment Test");
        }

        private void LoadSolutions()
        {
            dgvSolutions.Rows.Clear();

            foreach (var solution in solutions)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvSolutions);

                row.Cells[0].Value = solution.FriendlyName;
                row.Cells[1].Value = solution.Version;
                row.Cells[2].Value = solution.Version;

                // ✅ Important: Set to exact string from combo box items
                row.Cells[3].Value = "None";

                row.Tag = solution;
                dgvSolutions.Rows.Add(row);
            }

            cmbIncrementType.SelectedIndex = 0; // Select "None"
        }
        // Test in btnApplyToAll_Click to debug
        private void btnApplyToAll_Click(object sender, EventArgs e)
        {
            if (cmbIncrementType.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select an increment type.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedText = cmbIncrementType.SelectedItem.ToString();
            var incrementType = GetIncrementTypeFromIndex(cmbIncrementType.SelectedIndex);

            // Debug: Show what was selected
            MessageBox.Show($"Selected: {selectedText}\nIndex: {cmbIncrementType.SelectedIndex}\nType: {incrementType}",
                "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (DataGridViewRow row in dgvSolutions.Rows)
            {
                if (row.IsNewRow) continue;

                var solution = row.Tag as SolutionInfo;
                if (solution != null)
                {
                    var oldVersion = solution.Version;
                    var newVersion = solutionService.IncrementVersion(solution.Version, incrementType);

                    // Debug: Log version change
                    System.Diagnostics.Debug.WriteLine($"Old: {oldVersion}, New: {newVersion}, Type: {incrementType}");

                    row.Cells[2].Value = newVersion;
                    row.Cells[3].Value = selectedText; // Use exact selected text
                    row.Cells[2].Style.BackColor = Color.LightYellow;
                }
            }

            MessageBox.Show($"Applied {selectedText} to all solutions.",
                "Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private SolutionService.VersionIncrementType GetIncrementTypeFromIndex(int index)
        {
            // Index 0 = "None"
            // Index 1 = "Major (x.0.0.0)"
            // Index 2 = "Minor (0.x.0.0)"
            // Index 3 = "Build (0.0.x.0)"
            // Index 4 = "Revision (0.0.0.x)"

            switch (index)
            {
                case 1:
                    return SolutionService.VersionIncrementType.Major;
                case 2:
                    return SolutionService.VersionIncrementType.Minor;
                case 3:
                    return SolutionService.VersionIncrementType.Build;
                case 4:
                    return SolutionService.VersionIncrementType.Revision;
                default:
                    return SolutionService.VersionIncrementType.Major; // fallback
            }
        }
        private SolutionService.VersionIncrementType GetIncrementTypeFromString(string incrementString)
        {
            if (string.IsNullOrEmpty(incrementString))
            {
                return SolutionService.VersionIncrementType.Major;
            }

            // ✅ Check in reverse order (most specific first) to avoid partial matches

            // Must check "Revision" BEFORE checking for other patterns
            if (incrementString.IndexOf("Revision", StringComparison.OrdinalIgnoreCase) >= 0 ||
                incrementString.Contains("0.0.0.x"))
            {
                return SolutionService.VersionIncrementType.Revision;
            }

            if (incrementString.IndexOf("Build", StringComparison.OrdinalIgnoreCase) >= 0 ||
                incrementString.Contains("0.0.x.0"))
            {
                return SolutionService.VersionIncrementType.Build;
            }

            if (incrementString.IndexOf("Minor", StringComparison.OrdinalIgnoreCase) >= 0 ||
                incrementString.Contains("0.x.0.0"))
            {
                return SolutionService.VersionIncrementType.Minor;
            }

            if (incrementString.IndexOf("Major", StringComparison.OrdinalIgnoreCase) >= 0 ||
                incrementString.Contains("x.0.0.0"))
            {
                return SolutionService.VersionIncrementType.Major;
            }

            // Default fallback
            return SolutionService.VersionIncrementType.Major;
        }
        private void dgvSolutions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Suppress the default error dialog
            e.ThrowException = false;
            e.Cancel = true;

            // Optionally log or handle the error
            if (e.ColumnIndex == 3) // Auto Increment column
            {
                // Reset to "None" if invalid value
                dgvSolutions.Rows[e.RowIndex].Cells[3].Value = "None";
            }
        }
        private void dgvSolutions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // ✅ Prevent recursive calls and only process user edits
            if (isUpdatingCell) return;
            if (!isUserEdit) return; // Only process when user is actively editing

            try
            {
                isUpdatingCell = true;

                // Handle auto increment column change
                if (e.ColumnIndex == 3) // Auto Increment column
                {
                    var incrementValue = dgvSolutions.Rows[e.RowIndex].Cells[3].Value?.ToString();

                    if (!string.IsNullOrEmpty(incrementValue) && incrementValue != "None")
                    {
                        var solution = dgvSolutions.Rows[e.RowIndex].Tag as SolutionInfo;
                        if (solution != null)
                        {
                            SolutionService.VersionIncrementType incrementType = GetIncrementTypeFromString(incrementValue);
                            var newVersion = solutionService.IncrementVersion(solution.Version, incrementType);

                            dgvSolutions.Rows[e.RowIndex].Cells[2].Value = newVersion;
                            dgvSolutions.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightYellow;
                        }
                    }
                }

                // Handle manual version entry
                if (e.ColumnIndex == 2) // New Version column
                {
                    var newVersion = dgvSolutions.Rows[e.RowIndex].Cells[2].Value?.ToString();

                    if (!string.IsNullOrEmpty(newVersion))
                    {
                        if (IsValidVersion(newVersion))
                        {
                            dgvSolutions.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightGreen;
                            dgvSolutions.Rows[e.RowIndex].Cells[3].Value = "Manual";
                        }
                        else
                        {
                            dgvSolutions.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightCoral;
                            MessageBox.Show("Invalid version format. Please use format: X.X.X.X",
                                "Invalid Version", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            finally
            {
                isUpdatingCell = false;
            }
        }
        private bool IsValidVersion(string version)
        {
            try
            {
                var v = new Version(version);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Validate all versions
            bool allValid = true;
            UpdatedVersions.Clear();

            foreach (DataGridViewRow row in dgvSolutions.Rows)
            {
                if (row.IsNewRow) continue;

                var solution = row.Tag as SolutionInfo;
                var newVersion = row.Cells[2].Value?.ToString();

                if (solution != null && !string.IsNullOrEmpty(newVersion))
                {
                    if (!IsValidVersion(newVersion))
                    {
                        allValid = false;
                        row.Cells[2].Style.BackColor = Color.LightCoral;
                    }
                    else
                    {
                        UpdatedVersions[solution.UniqueName] = newVersion;
                    }
                }
            }

            if (!allValid)
            {
                MessageBox.Show("Some versions are invalid. Please correct them before proceeding.",
                    "Invalid Versions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if any versions were actually changed
            bool anyChanges = false;
            foreach (DataGridViewRow row in dgvSolutions.Rows)
            {
                if (row.IsNewRow) continue;

                var currentVersion = row.Cells[1].Value?.ToString();
                var newVersion = row.Cells[2].Value?.ToString();

                if (currentVersion != newVersion)
                {
                    anyChanges = true;
                    break;
                }
            }

            if (!anyChanges)
            {
                var result = MessageBox.Show(
                    "No version changes detected. Do you want to continue without updating versions?",
                    "No Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            UpdateInSource = chkUpdateInSource.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
