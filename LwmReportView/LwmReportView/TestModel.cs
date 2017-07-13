using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LwmReportView
{
    public class TestModel
    {
        public int 编号 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品描述 { get; set; }
        public double 价格 { get; set; }
        public int 数量 { get; set; }
        public DateTime? 下单日期 { get; set; }
        public int 库存 { get; set; }
        public bool? 库存状态
        {
            get { return 库存 < 300; }
        }
    }

    public static class DoddleProductRepository
    {
        public static List<TestModel> GetAll()
        {
            var rand = new Random();
            return Enumerable.Range(1, 200)
                .Select(i => new TestModel
                {
                    编号 = i,
                    商品名称 = "商品" + i,
                    商品描述 = "在某些项目中显示长文本",
                    价格 = rand.NextDouble() * 100,
                    数量 = rand.Next(1000),
                    下单日期 = DateTime.Now.AddDays(rand.Next(1000)),
                    库存 = rand.Next(0, 1000)
                })
                .ToList();
        }

        public static IEnumerable<ExpandoObject> GetAllExpando()
        {
            foreach (var product in GetAll())
            {
                dynamic item = new ExpandoObject();

                item.编号 = product.编号;
                item.Name = product.商品名称;
                item.Description = product.商品描述 + " (dynamic)";
                item.Price = product.价格;
                item.OrderCount = product.数量;
                item.LastPuchase = product.下单日期;
                item.UnitsInStock = product.库存;
                item.LowStock = product.库存状态;
                yield return item;
            }
        }
    }
}
