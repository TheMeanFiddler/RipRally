using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public static class BillOfRoadMaterials
    {
    public static ShopItem[] Items
    {
        get
        {
            var RdShopItms = Shop.Items.Where(i => i.Type == ShopItemType.Road).Select(i => new
            {
                Mat = i.Name,
                Img = i.Image,
                Cost = i.Cost
            });
            var ChargeableRdCount =
                    Road.Instance.Sectns.Where(s => s.Chargeable == true)
                    .GroupBy(s => s.RoadMaterial)
                    .Select(cl => new
                    {
                        Mat = cl.First().RoadMaterial,
                        SectnCount = cl.Count()
                    });

            var ChargeableRdCost =
                from rs in ChargeableRdCount
                join ShItm in RdShopItms on rs.Mat equals ShItm.Mat
                select new ShopItem(ShopItemType.Road, ShItm.Mat, "", 0, rs.SectnCount * ShItm.Cost);

            return ChargeableRdCost.ToArray();
        }
    }
}

public static class BillOfSceneryMaterials
{
    public static PlaceableObject[] Items { get
        {
            return Scenery.Instance.Objects.Where(ScO => ScO.Chargeable == true).ToArray();
        }
    }
}

