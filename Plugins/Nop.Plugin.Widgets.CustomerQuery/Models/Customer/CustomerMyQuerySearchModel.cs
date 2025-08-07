using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Customer;
public partial record CustomerMyQuerySearchModel : BaseSearchModel
{   
    public string SearchSubject { get; set; }
    public string SearchMessage { get; set; }
    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.List.SearchCreatedOnFrom")]
    [UIHint("DateNullable")] // This will show only date picker
    public DateTime? SearchCreatedOnFrom { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.List.SearchCreatedOnTo")]
    [UIHint("DateNullable")] // This will show only date picker
    public DateTime? SearchCreatedOnTo { get; set; }
    public bool SearchShowHidden { get; set; } = true;
    
}
