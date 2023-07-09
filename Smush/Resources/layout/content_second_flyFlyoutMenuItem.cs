using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smush.Resources.layout
{
    public class content_second_flyFlyoutMenuItem
    {
        public content_second_flyFlyoutMenuItem()
        {
            TargetType = typeof(content_second_flyFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}