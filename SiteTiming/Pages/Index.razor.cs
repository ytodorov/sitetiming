using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaywrightTestLinuxContainer;
using PlaywrightTestLinuxContainer.Entities;
using Microsoft.EntityFrameworkCore;

namespace SiteTiming.Pages
{
    public partial class Index
    {
        [Inject]
        public SiteTimingContext SiteTimingContext { get; set; }
        
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        public string Now { get; set; } = DateTime.Now.ToString("O");

        public List<TimingEntity> Timings { get; set; }

        public List<TimingEntity> Timings0 { get; set; } = new List<TimingEntity>();

        public List<TimingEntity> Timings1 { get; set; } = new List<TimingEntity>();

        public List<TimingEntity> Timings2 { get; set; } = new List<TimingEntity>();

        protected override void OnInitialized()
        {
            var isInitializedProp = JsRuntime.GetType().GetProperties().FirstOrDefault(f => f.Name == "IsInitialized");
            bool isJsInitialized = (bool)isInitializedProp.GetValue(JsRuntime);
            if (isJsInitialized == true)
            {
                Timings = SiteTimingContext.Timings.Include(s => s.Site).OrderByDescending(s => s.DateCreated).Take(3)
                    .GroupBy(s => s.SiteId).Select(s => s.OrderByDescending(s => s.DateCreated).First())
                    .Take(3) // speed 
                    .ToList()
                    .OrderByDescending(s => s.DateCreated)
                    .ToList();

                for (int i = 0; i < Timings.Count; i++)
                {
                    var t = Timings[i];
                    if (i % 3 == 0)
                    {
                        Timings0.Add(t);
                    }
                    if (i % 3 == 1)
                    {
                        Timings1.Add(t);
                    }
                    if (i % 3 == 2)
                    {
                        Timings2.Add(t);
                    }
                }

            }

            base.OnInitialized();
        }
    }
}
