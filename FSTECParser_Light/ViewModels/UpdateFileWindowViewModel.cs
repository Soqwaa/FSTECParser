using FSTECParser_Light.Models;
using System.Collections.Generic;
using System.Linq;

namespace FSTECParser_Light.ViewModels
{
    class UpdateFileWindowViewModel : ViewModelBase
    {
        public static List<Change> _changes;
        private  static int changesCount = 0;
        public int ChangesCount
        {
            get
            {
                return changesCount;
            }
            set
            {
                changesCount = value;
                OnPropertyChanged("changesCount");
            }
        }
        public List<Change> Changes
        {
            get
            {
                return _changes;
            }
            set
            {
                _changes = value;
                OnPropertyChanged("_changes");
            }
        }
        public UpdateFileWindowViewModel(List<Change> changes)
        {
            Changes = changes;
            ChangesCount = changes.Select(x => x.Id).Distinct().Count();
            new UpdateFileWindow().Show();
        }
        // Для дизайнера
        public UpdateFileWindowViewModel() { }
    }
}
