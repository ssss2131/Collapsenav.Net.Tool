using Xunit;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Collapsenav.Net.Tool.Test
{
    public class FilesTest
    {
        [Fact]
        public async Task GetFileTest()
        {
            var dd = await FileTool.GetAsync(@"http://202.182.125.80:9090/api/File/download/2bca1ba9-19b3-4691-ba46-385d91aef7d7");
            var result = await dd.SaveToAsync("/index.mp3", true);
        }
    }
}
