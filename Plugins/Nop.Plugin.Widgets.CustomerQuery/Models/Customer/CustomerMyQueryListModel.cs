using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Customer;
public partial record CustomerMyQueryListModel : BaseNopModel
{


    public CustomerMyQueryListModel()
    {
        CustomerQueries = new List<CustomerQueryModel>();
    }
    public IList<CustomerQueryModel> CustomerQueries { get; set; }
 
    

}
