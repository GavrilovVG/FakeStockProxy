using System.Text.Json;

namespace FakeStockProxy.UnitTests.Builders.Data;

public class FsStockRequestDtoBuilder
{
    public FsResponseDataDto DataExampleResponseSuccess { get; init; }
    public FsResponseDataDto DataExampleResponseFailure { get; init; }
    private Random _random;

    public FsStockRequestDtoBuilder()
    {
        DataExampleResponseSuccess = JsonSerializer.Deserialize<FsResponseDataDto>(JsonResultExample)!;
        DataExampleResponseFailure = new FsResponseDataDto();
        _random = new Random(100);
    }

    public FsStockRequestDto Build()
    {
        FsStockRequestDto retval = new();

        retval.Skip = _random.Next(0, 30);
        retval.Take = _random.Next(1, 5) * 10;

        if (_random.Next() % 2 == 0)
            retval.Filter = "title='" + DataExampleResponseSuccess.FsStock!.FsStockItems![_random.Next(DataExampleResponseSuccess.FsStock.FsStockItems.Count - 1)].Title + "'";
        else
            retval.Filter = "";

        if (_random.Next() % 2 == 0)
        {
            if (!string.IsNullOrEmpty(retval.Filter)) retval.Filter += "&";
            retval.Filter += "manufacturer='" + DataExampleResponseSuccess.FsStock!.FsStockItems![_random.Next(DataExampleResponseSuccess.FsStock.FsStockItems.Count - 1)].Manufacturer + "'";
        }

        if (_random.Next() % 2 == 0)
        {
            if (!string.IsNullOrEmpty(retval.Filter)) retval.Filter += "&";
            retval.Filter += "description='" + DataExampleResponseSuccess.FsStock!.FsStockItems![_random.Next(DataExampleResponseSuccess.FsStock.FsStockItems.Count - 1)].Description + "'";
        }

        if (_random.Next() % 2 == 0)
        {
            retval.OrderBy = orderByFields[_random.Next(orderByFields.Count - 1)];
            retval.OrderDirection = orderByDirections[_random.Next(orderByDirections.Count - 1)];
        }

        retval.SetHashsum();

        return retval;
    }

    public List<FsStockRequestDto> BuildList(int itemsCount)
    {
        var retval = Enumerable.Range(1, itemsCount).Select(i => Build()).ToList();

        return retval;
    }

    private List<string> orderByFields = new List<string>() { "title", "price", "stock" };
    private List<string> orderByDirections = new List<string>() { "asc", "desc" };

    private const string JsonResultExample = @"{
          ""result"": {
            ""totalItems"": 300,
            ""items"": [
              {
                ""code"": ""2716"",
                ""title"": ""a neque. Nullam ut nisi"",
                ""manufacturer"": ""Pharetra Sed Hendrerit LLC"",
                ""description"": ""elit. Curabitur sed tortor. Integer aliquam adipiscing lacus. Ut nec urna et arcu imperdiet ullamcorper. Duis"",
                ""price"": ""€172,430"",
                ""stock"": 16
              },
              {
                ""code"": ""Z8132"",
                ""title"": ""a nunc. In at"",
                ""manufacturer"": ""Non Dui Corp."",
                ""description"": ""arcu imperdiet ullamcorper. Duis at lacus. Quisque purus sapien, gravida non, sollicitudin a, malesuada id, erat. Etiam vestibulum massa rutrum magna. Cras convallis convallis dolor. Quisque tincidunt pede ac urna."",
                ""price"": ""€159,398"",
                ""stock"": 49
              },
              {
                ""code"": ""2072"",
                ""title"": ""ac metus vitae velit egestas"",
                ""manufacturer"": ""Tincidunt Company"",
                ""description"": ""Cras dolor dolor, tempus non, lacinia at, iaculis quis, pede. Praesent eu dui. Cum sociis natoque penatibus et magnis dis parturient"",
                ""price"": ""€161,621"",
                ""stock"": 6
              },
              {
                ""code"": ""02759"",
                ""title"": ""ac mi eleifend egestas."",
                ""manufacturer"": ""Semper Nam LLP"",
                ""description"": ""lacinia mattis. Integer eu lacus. Quisque imperdiet, erat nonummy ultricies ornare, elit elit fermentum risus, at fringilla purus mauris a nunc."",
                ""price"": ""€153,492"",
                ""stock"": 28
              },
              {
                ""code"": ""34-548"",
                ""title"": ""ac urna. Ut"",
                ""manufacturer"": ""Suspendisse Sed Dolor Associates"",
                ""description"": ""ipsum ac mi eleifend egestas. Sed pharetra, felis eget varius ultrices, mauris ipsum porta elit,"",
                ""price"": ""€133,267"",
                ""stock"": 4
              },
              {
                ""code"": ""888940"",
                ""title"": ""ac, feugiat non, lobortis"",
                ""manufacturer"": ""Integer Ltd"",
                ""description"": ""posuere at, velit. Cras lorem lorem, luctus ut, pellentesque eget, dictum placerat, augue. Sed molestie. Sed id"",
                ""price"": ""€102,805"",
                ""stock"": 47
              },
              {
                ""code"": ""6927"",
                ""title"": ""accumsan convallis, ante"",
                ""manufacturer"": ""Proin Velit Consulting"",
                ""description"": ""aliquet vel, vulputate eu, odio. Phasellus at augue id ante dictum cursus. Nunc mauris elit, dictum eu, eleifend nec, malesuada ut, sem. Nulla interdum. Curabitur dictum."",
                ""price"": ""€135,178"",
                ""stock"": 23
              },
              {
                ""code"": ""5697"",
                ""title"": ""adipiscing elit. Curabitur"",
                ""manufacturer"": ""Augue Eu Incorporated"",
                ""description"": ""lectus convallis est, vitae sodales nisi magna sed dui. Fusce aliquam, enim nec tempus scelerisque, lorem ipsum sodales"",
                ""price"": ""€156,781"",
                ""stock"": 1
              },
              {
                ""code"": ""75266"",
                ""title"": ""adipiscing non, luctus sit amet,"",
                ""manufacturer"": ""Magna Malesuada Inc."",
                ""description"": ""at pede. Cras vulputate velit eu sem. Pellentesque ut ipsum ac mi eleifend egestas. Sed pharetra, felis eget varius ultrices, mauris ipsum porta elit, a feugiat tellus lorem eu"",
                ""price"": ""€171,087"",
                ""stock"": 23
              },
              {
                ""code"": ""13122"",
                ""title"": ""adipiscing. Mauris molestie pharetra"",
                ""manufacturer"": ""Mauris Blandit Corp."",
                ""description"": ""Nullam ut nisi a odio semper cursus. Integer mollis. Integer tincidunt aliquam arcu. Aliquam ultrices iaculis odio. Nam interdum enim non nisi. Aenean eget"",
                ""price"": ""€148,220"",
                ""stock"": 42
              },
              {
                ""code"": ""678557"",
                ""title"": ""Aenean eget magna."",
                ""manufacturer"": ""Aliquet Inc."",
                ""description"": ""nostra, per inceptos hymenaeos. Mauris ut quam vel sapien imperdiet ornare. In faucibus. Morbi"",
                ""price"": ""€124,928"",
                ""stock"": 34
              },
              {
                ""code"": ""211245"",
                ""title"": ""aliquam eros turpis"",
                ""manufacturer"": ""Felis Purus Ac Industries"",
                ""description"": ""eu turpis. Nulla aliquet. Proin velit. Sed malesuada augue ut lacus. Nulla tincidunt, neque vitae semper egestas, urna justo faucibus lectus, a sollicitudin orci sem eget massa. Suspendisse eleifend."",
                ""price"": ""€138,747"",
                ""stock"": 13
              },
              {
                ""code"": ""02406"",
                ""title"": ""Aliquam gravida mauris"",
                ""manufacturer"": ""Ullamcorper Consulting"",
                ""description"": ""dui, semper et, lacinia vitae, sodales at, velit. Pellentesque ultricies dignissim lacus. Aliquam rutrum lorem ac risus. Morbi metus. Vivamus euismod urna. Nullam lobortis quam a felis"",
                ""price"": ""€150,498"",
                ""stock"": 24
              },
              {
                ""code"": ""5332"",
                ""title"": ""aliquet lobortis, nisi"",
                ""manufacturer"": ""Lacinia Associates"",
                ""description"": ""sit amet orci. Ut sagittis lobortis mauris. Suspendisse aliquet molestie tellus. Aenean egestas hendrerit neque. In"",
                ""price"": ""€121,920"",
                ""stock"": 49
              },
              {
                ""code"": ""25438"",
                ""title"": ""aliquet, metus urna convallis"",
                ""manufacturer"": ""Hymenaeos Foundation"",
                ""description"": ""Sed eu eros. Nam consequat dolor vitae dolor. Donec fringilla. Donec feugiat metus sit amet ante. Vivamus non lorem vitae"",
                ""price"": ""€162,588"",
                ""stock"": 28
              },
              {
                ""code"": ""20615"",
                ""title"": ""aliquet. Phasellus fermentum convallis ligula."",
                ""manufacturer"": ""Metus Facilisis LLC"",
                ""description"": ""leo. Cras vehicula aliquet libero. Integer in magna. Phasellus dolor elit, pellentesque a, facilisis non,"",
                ""price"": ""€154,208"",
                ""stock"": 44
              },
              {
                ""code"": ""85595-80353"",
                ""title"": ""amet ultricies sem magna nec"",
                ""manufacturer"": ""Phasellus Nulla Company"",
                ""description"": ""malesuada ut, sem. Nulla interdum. Curabitur dictum. Phasellus in felis. Nulla tempor augue ac ipsum. Phasellus vitae mauris"",
                ""price"": ""€195,457"",
                ""stock"": 6
              },
              {
                ""code"": ""69093"",
                ""title"": ""amet, consectetuer adipiscing elit."",
                ""manufacturer"": ""Ac LLP"",
                ""description"": ""Proin ultrices. Duis volutpat nunc sit amet metus. Aliquam erat"",
                ""price"": ""€116,251"",
                ""stock"": 43
              },
              {
                ""code"": ""68459"",
                ""title"": ""ante dictum mi,"",
                ""manufacturer"": ""Vulputate Dui Nec Corp."",
                ""description"": ""enim. Mauris quis turpis vitae purus gravida sagittis. Duis gravida. Praesent eu"",
                ""price"": ""€135,888"",
                ""stock"": 28
              },
              {
                ""code"": ""553397"",
                ""title"": ""ante, iaculis nec, eleifend"",
                ""manufacturer"": ""Quam Quis Limited"",
                ""description"": ""massa. Mauris vestibulum, neque sed dictum eleifend, nunc risus varius orci,"",
                ""price"": ""€180,333"",
                ""stock"": 34
              },
              {
                ""code"": ""Z9889"",
                ""title"": ""arcu imperdiet ullamcorper."",
                ""manufacturer"": ""Sed Dictum Eleifend Limited"",
                ""description"": ""lobortis mauris. Suspendisse aliquet molestie tellus. Aenean egestas hendrerit neque. In ornare sagittis felis. Donec tempor, est ac mattis semper, dui lectus rutrum urna, nec luctus felis purus"",
                ""price"": ""€143,263"",
                ""stock"": 2
              },
              {
                ""code"": ""JF2 2HK"",
                ""title"": ""arcu vel quam dignissim"",
                ""manufacturer"": ""Ac Arcu Nunc Corp."",
                ""description"": ""aliquam iaculis, lacus pede sagittis augue, eu tempor erat neque non quam. Pellentesque habitant morbi tristique senectus et netus et malesuada fames"",
                ""price"": ""€178,836"",
                ""stock"": 31
              },
              {
                ""code"": ""388762"",
                ""title"": ""arcu. Sed et libero."",
                ""manufacturer"": ""Donec Elementum PC"",
                ""description"": ""Sed auctor odio a purus. Duis elementum, dui quis accumsan convallis, ante lectus convallis est, vitae"",
                ""price"": ""€159,201"",
                ""stock"": 43
              },
              {
                ""code"": ""423484"",
                ""title"": ""arcu. Sed eu nibh vulputate"",
                ""manufacturer"": ""Ut Company"",
                ""description"": ""malesuada. Integer id magna et ipsum cursus vestibulum. Mauris magna. Duis dignissim tempor arcu. Vestibulum ut eros non enim commodo hendrerit. Donec porttitor tellus non"",
                ""price"": ""€124,389"",
                ""stock"": 13
              },
              {
                ""code"": ""848440"",
                ""title"": ""at auctor ullamcorper, nisl arcu"",
                ""manufacturer"": ""Cras Eu Tellus Ltd"",
                ""description"": ""adipiscing ligula. Aenean gravida nunc sed pede. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Proin vel arcu eu odio tristique pharetra. Quisque ac"",
                ""price"": ""€185,729"",
                ""stock"": 35
              },
              {
                ""code"": ""562427"",
                ""title"": ""at risus. Nunc ac sem"",
                ""manufacturer"": ""Suscipit Institute"",
                ""description"": ""Cras eu tellus eu augue porttitor interdum. Sed auctor odio a purus. Duis elementum, dui quis accumsan convallis, ante lectus convallis est, vitae sodales nisi magna sed dui. Fusce"",
                ""price"": ""€111,129"",
                ""stock"": 11
              },
              {
                ""code"": ""79-047"",
                ""title"": ""at, velit. Cras lorem"",
                ""manufacturer"": ""Ornare Fusce Mollis Corp."",
                ""description"": ""ligula tortor, dictum eu, placerat eget, venenatis a, magna. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Etiam laoreet, libero et tristique pellentesque, tellus sem mollis"",
                ""price"": ""€194,572"",
                ""stock"": 41
              },
              {
                ""code"": ""5897"",
                ""title"": ""auctor velit. Aliquam"",
                ""manufacturer"": ""Rutrum Fusce Dolor Industries"",
                ""description"": ""sem ut dolor dapibus gravida. Aliquam tincidunt, nunc ac mattis ornare, lectus ante dictum mi, ac mattis velit justo nec ante. Maecenas mi felis, adipiscing fringilla, porttitor vulputate, posuere"",
                ""price"": ""€192,958"",
                ""stock"": 0
              },
              {
                ""code"": ""A4B 5P9"",
                ""title"": ""augue eu tellus. Phasellus"",
                ""manufacturer"": ""Non Arcu Vivamus Consulting"",
                ""description"": ""mauris sagittis placerat. Cras dictum ultricies ligula. Nullam enim. Sed nulla ante, iaculis nec, eleifend non, dapibus rutrum, justo. Praesent luctus. Curabitur egestas nunc sed libero."",
                ""price"": ""€186,332"",
                ""stock"": 7
              },
              {
                ""code"": ""41616"",
                ""title"": ""augue, eu tempor"",
                ""manufacturer"": ""Sit PC"",
                ""description"": ""a, auctor non, feugiat nec, diam. Duis mi enim, condimentum eget, volutpat ornare, facilisis eget, ipsum. Donec sollicitudin adipiscing ligula. Aenean gravida nunc sed pede. Cum sociis natoque penatibus"",
                ""price"": ""€167,013"",
                ""stock"": 20
              },
              {
                ""code"": ""155325"",
                ""title"": ""augue, eu tempor erat"",
                ""manufacturer"": ""Ultrices Mauris Ipsum Ltd"",
                ""description"": ""lorem, sit amet ultricies sem magna nec quam. Curabitur vel lectus. Cum sociis natoque penatibus et magnis dis"",
                ""price"": ""€144,599"",
                ""stock"": 50
              },
              {
                ""code"": ""7591"",
                ""title"": ""blandit at, nisi. Cum sociis"",
                ""manufacturer"": ""Luctus Lobortis Class Ltd"",
                ""description"": ""fringilla mi lacinia mattis. Integer eu lacus. Quisque imperdiet, erat nonummy ultricies ornare, elit elit fermentum risus, at fringilla purus mauris a nunc. In at"",
                ""price"": ""€118,813"",
                ""stock"": 29
              },
              {
                ""code"": ""76291"",
                ""title"": ""consectetuer adipiscing elit. Etiam"",
                ""manufacturer"": ""Proin Velit Sed PC"",
                ""description"": ""lobortis risus. In mi pede, nonummy ut, molestie in, tempus eu, ligula. Aenean euismod mauris eu elit."",
                ""price"": ""€113,049"",
                ""stock"": 42
              },
              {
                ""code"": ""5457"",
                ""title"": ""consectetuer rhoncus. Nullam"",
                ""manufacturer"": ""Tellus Nunc Lectus Limited"",
                ""description"": ""In condimentum. Donec at arcu. Vestibulum ante ipsum primis in faucibus orci"",
                ""price"": ""€163,343"",
                ""stock"": 39
              },
              {
                ""code"": ""980314"",
                ""title"": ""consectetuer, cursus et,"",
                ""manufacturer"": ""Morbi Tristique Senectus LLP"",
                ""description"": ""Aliquam ultrices iaculis odio. Nam interdum enim non nisi. Aenean eget metus. In nec orci."",
                ""price"": ""€114,909"",
                ""stock"": 33
              },
              {
                ""code"": ""41502"",
                ""title"": ""consectetuer, cursus et, magna."",
                ""manufacturer"": ""Vivamus Nisi Mauris Corporation"",
                ""description"": ""dui. Suspendisse ac metus vitae velit egestas lacinia. Sed congue, elit sed consequat auctor, nunc nulla vulputate dui, nec tempus"",
                ""price"": ""€113,154"",
                ""stock"": 37
              },
              {
                ""code"": ""66110"",
                ""title"": ""convallis ligula. Donec luctus aliquet"",
                ""manufacturer"": ""At Arcu Inc."",
                ""description"": ""eget tincidunt dui augue eu tellus. Phasellus elit pede, malesuada vel, venenatis vel, faucibus id, libero. Donec consectetuer mauris id sapien. Cras dolor dolor, tempus non, lacinia"",
                ""price"": ""€122,111"",
                ""stock"": 50
              },
              {
                ""code"": ""4986"",
                ""title"": ""Cras dictum ultricies ligula."",
                ""manufacturer"": ""Luctus LLC"",
                ""description"": ""est, congue a, aliquet vel, vulputate eu, odio. Phasellus at augue id ante dictum cursus. Nunc mauris elit, dictum"",
                ""price"": ""€132,263"",
                ""stock"": 31
              },
              {
                ""code"": ""5809"",
                ""title"": ""Curabitur dictum. Phasellus in"",
                ""manufacturer"": ""Orci LLP"",
                ""description"": ""natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aenean eget magna. Suspendisse tristique neque venenatis lacus. Etiam bibendum fermentum metus. Aenean sed pede nec ante blandit"",
                ""price"": ""€188,570"",
                ""stock"": 9
              },
              {
                ""code"": ""37447"",
                ""title"": ""Curae; Donec tincidunt. Donec vitae"",
                ""manufacturer"": ""Velit Quisque Varius Institute"",
                ""description"": ""mus. Proin vel nisl. Quisque fringilla euismod enim. Etiam gravida molestie arcu. Sed eu nibh vulputate mauris sagittis placerat. Cras"",
                ""price"": ""€171,899"",
                ""stock"": 14
              },
              {
                ""code"": ""9846 TH"",
                ""title"": ""cursus a, enim."",
                ""manufacturer"": ""Pede Associates"",
                ""description"": ""et, commodo at, libero. Morbi accumsan laoreet ipsum. Curabitur consequat,"",
                ""price"": ""€147,345"",
                ""stock"": 5
              },
              {
                ""code"": ""C4P 6V3"",
                ""title"": ""dapibus gravida. Aliquam tincidunt,"",
                ""manufacturer"": ""Morbi Vehicula Pellentesque LLP"",
                ""description"": ""sodales elit erat vitae risus. Duis a mi fringilla mi lacinia mattis. Integer eu lacus. Quisque imperdiet, erat nonummy ultricies ornare,"",
                ""price"": ""€169,508"",
                ""stock"": 28
              },
              {
                ""code"": ""047438"",
                ""title"": ""dapibus id, blandit"",
                ""manufacturer"": ""Elit Pretium Et Inc."",
                ""description"": ""felis. Donec tempor, est ac mattis semper, dui lectus rutrum urna, nec luctus felis purus ac tellus. Suspendisse sed dolor. Fusce mi lorem, vehicula et, rutrum eu, ultrices"",
                ""price"": ""€105,780"",
                ""stock"": 37
              },
              {
                ""code"": ""360144"",
                ""title"": ""diam eu dolor"",
                ""manufacturer"": ""Duis Gravida Praesent Ltd"",
                ""description"": ""Mauris non dui nec urna suscipit nonummy. Fusce fermentum fermentum arcu. Vestibulum ante ipsum primis"",
                ""price"": ""€158,771"",
                ""stock"": 50
              },
              {
                ""code"": ""261582"",
                ""title"": ""diam luctus lobortis. Class aptent"",
                ""manufacturer"": ""Eros Nec Tellus Associates"",
                ""description"": ""imperdiet dictum magna. Ut tincidunt orci quis lectus. Nullam suscipit, est ac facilisis facilisis,"",
                ""price"": ""€139,430"",
                ""stock"": 11
              },
              {
                ""code"": ""128753"",
                ""title"": ""dictum augue malesuada"",
                ""manufacturer"": ""Cras Dictum Ltd"",
                ""description"": ""nisl. Maecenas malesuada fringilla est. Mauris eu turpis. Nulla aliquet. Proin velit. Sed malesuada augue ut lacus. Nulla tincidunt, neque vitae semper egestas, urna justo faucibus lectus, a"",
                ""price"": ""€166,234"",
                ""stock"": 9
              },
              {
                ""code"": ""Z24 1VQ"",
                ""title"": ""dictum eu, eleifend nec, malesuada"",
                ""manufacturer"": ""Lacinia At Institute"",
                ""description"": ""id, mollis nec, cursus a, enim. Suspendisse aliquet, sem ut cursus luctus, ipsum leo elementum sem, vitae aliquam eros"",
                ""price"": ""€142,926"",
                ""stock"": 23
              },
              {
                ""code"": ""111975"",
                ""title"": ""dictum. Phasellus in"",
                ""manufacturer"": ""Dui Cum Sociis Limited"",
                ""description"": ""Phasellus nulla. Integer vulputate, risus a ultricies adipiscing, enim mi"",
                ""price"": ""€149,560"",
                ""stock"": 20
              },
              {
                ""code"": ""Z1290"",
                ""title"": ""dictum. Proin eget"",
                ""manufacturer"": ""Nulla Inc."",
                ""description"": ""sed tortor. Integer aliquam adipiscing lacus. Ut nec urna et arcu imperdiet ullamcorper. Duis at lacus. Quisque purus sapien,"",
                ""price"": ""€179,630"",
                ""stock"": 4
              },
              {
                ""code"": ""03290"",
                ""title"": ""dis parturient montes, nascetur ridiculus"",
                ""manufacturer"": ""Sed Neque Inc."",
                ""description"": ""ipsum porta elit, a feugiat tellus lorem eu metus. In lorem. Donec"",
                ""price"": ""€121,182"",
                ""stock"": 11
              }
            ]
          },
          ""status"": ""Ok"",
          ""requestId"": ""fa6aebcc-bf7e-48ab-8fa6-d96c8aab041d""
        }";
}
