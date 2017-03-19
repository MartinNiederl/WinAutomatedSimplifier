using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.PasswordProtectedFolder;

namespace WindowsAutomatedSimplifier.Repository.TreeViewWithCheckBoxes
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        private bool? _isChecked = false;
        private TreeViewModel _parent;

        public List<TreeViewModel> Children { get; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; }

        public Header FileInfo { get; }

        public static List<TreeViewModel> SetContent()
        {
            TreeViewModel root = new TreeViewModel("root") { IsInitiallySelected = true };

            foreach (Header h in FolderReader.Headers)
            {
                TreeViewModel tempRoot = root;
                string tempPath = h.Path.Substring(1);

                do
                {
                    int endIndex = tempPath.IndexOf('\\');
                    if (endIndex < 0)
                    {
                        tempRoot.Children.Add(new TreeViewModel(tempPath, h));
                        break;
                    }
                    string tempSub = tempPath.Substring(0, endIndex);
                    TreeViewModel tempRoot2 = tempRoot.Children.Find(a => a.Name == tempSub);

                    if (tempRoot2 != null) tempRoot = tempRoot2;
                    else
                    {
                        TreeViewModel newRoot = new TreeViewModel(tempSub);
                        tempRoot.Children.Add(newRoot);
                        tempRoot = newRoot;
                    }
                    tempPath = tempPath.Substring(endIndex + 1);
                } while (true);
            }

            root.Initialize();
            return new List<TreeViewModel> { root };
        }

        public static TreeViewModel FindSub(TreeViewModel node, string name)
        {
            if (node == null) return null;
            return node.Name == name ? node : node.Children.Select(child => FindSub(child, name)).FirstOrDefault(found => found != null);
        }

        private TreeViewModel(string name)
        {
            Name = name;
            Children = new List<TreeViewModel>();
        }
        private TreeViewModel(string name, Header header)
        {
            Name = name;
            Children = new List<TreeViewModel>();
            FileInfo = header;
        }

        private void Initialize()
        {
            foreach (TreeViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child TreeViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent) _parent?.VerifyCheckState();

            OnPropertyChanged("IsChecked");
        }

        private void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0) state = current;
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}