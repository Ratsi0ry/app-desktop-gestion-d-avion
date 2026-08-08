using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Gestion_avion.ViewModels;
using Gestion_avion.Views.Cards;

namespace Gestion_avion.Selectors
{
    public class TemplateFilter : IDataTemplate
    {
        // 1. handles your CardViewModel
        public bool Match(object? data)
        {
            return data is CardViewModel;
        }

        // 2. return the correct .axaml View dynamically
        public Control? Build(object? param)
        {
            if (param is CardViewModel vm)
            {
                if (vm.CardType == "user")
                {
                    return new UserCard { DataContext = vm };
                }
                else if (vm.CardType == "plane")
                {
                    return new PlaneCard { DataContext = vm };
                }
                else if (vm.CardType == "flight")
                {
                    return new FlightCard { DataContext = vm};
                }
            }
            
            return new TextBlock { Text = "Template Not Found" };
        }
    }
}
