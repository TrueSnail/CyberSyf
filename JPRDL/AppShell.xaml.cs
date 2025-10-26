using JPRDL.Views;

namespace JPRDL
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("user", typeof(UserPage));
            Routing.RegisterRoute("admin", typeof(AdminPage));
        }
    }
}
