using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class RelationModel
    {
        public string id { get; set; }
        public string relation_name { get; set; }
    }

    public class HomeRelationModel : INotifyPropertyChanged
    {
        #region Declarations
        private int _serial;
        private string _national_id;
        private string _contact_name;
        private string _contact_number;

        private RelationModel _selected_relation;
        private List<RelationModel> _lstRelations;
        #endregion

        #region Properties
        public int serial
        {
            get { return _serial; }
            set
            {
                _serial = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(serial)));
            }
        }

        public string national_id
        {
            get { return _national_id; }
            set
            {
                _national_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(national_id)));
            }
        }

        public string contact_name
        {
            get { return _contact_name; }
            set
            {
                _contact_name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(contact_name)));
            }
        }

        public string contact_number
        {
            get { return _contact_number; }
            set
            {
                _contact_number = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(contact_number)));
            }
        }

        public RelationModel selected_relation
        {
            get { return _selected_relation; }
            set
            {
                _selected_relation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(selected_relation)));
            }
        }

        public List<RelationModel> lstRelations
        {
            get { return _lstRelations; }
            set
            {
                _lstRelations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lstRelations)));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }

    public class RelationListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<RelationModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
