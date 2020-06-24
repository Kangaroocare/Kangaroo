using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessageDetailsPage : BasePage
	{

        #region Declarations
        private bool isTapped = false;

        private string recipient_id = "";
        #endregion

        #region Functions
        public MessageDetailsPage(string recipientId, string title)
        {
            InitializeComponent();

            recipient_id = recipientId;
            barTitle.Title = title;
            var vm = new MessageViewModel(Navigation);
            vm.OnGetMessageDetails(recipient_id);
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            
        }

        private async void cmdSend_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            var vm = this.BindingContext as MessageViewModel;
            var msg_count = vm.lstMessageDetails.Count();
            await vm.OnSendReplyMessage(recipient_id, txtReply.Text);
            await vm.OnGetMessageDetails(recipient_id);
            var oLast = vm.lstMessageDetails.LastOrDefault();
            lvMessages.ScrollTo(oLast, Syncfusion.ListView.XForms.ScrollToPosition.MakeVisible, true);
            txtReply.Text = "";

            isTapped = false;
        }
        #endregion

    }
}