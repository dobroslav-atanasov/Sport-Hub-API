namespace SportData.Converters;

using System.Text;

using Dasync.Collections;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportData.Common.Extensions;
using SportData.Data.Models.Entities.Crawlers;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;
using SportData.Services.Interfaces;

public abstract class BaseConverter
{
    private readonly ICrawlersService crawlersService;
    private readonly ILogsService logsService;
    private readonly IGroupsService groupsService;
    private readonly IZipService zipService;

    public BaseConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService)
    {
        this.Logger = logger;
        this.crawlersService = crawlersService;
        this.logsService = logsService;
        this.groupsService = groupsService;
        this.zipService = zipService;
    }

    protected ILogger<BaseConverter> Logger { get; }

    protected abstract Task ProcessGroupAsync(Group group);

    public async Task ConvertAsync(string crawlerName)
    {
        this.Logger.LogInformation($"Converter: {crawlerName} start.");

        try
        {
            var crawlerId = await this.crawlersService.GetCrawlerIdAsync(crawlerName);
            var identifiers = await this.logsService.GetLogIdentifiersAsync(crawlerId);

            identifiers = new List<Guid>
            {
Guid.Parse("1deaf00a-af11-4820-8201-10f4c7f32fc4"),
Guid.Parse("16d3c329-7fbb-477b-9280-791168efd31a"),
Guid.Parse("17f849d3-1ed0-4b20-8afa-940399ab9822"),
Guid.Parse("ded96ee8-e0a8-4328-8e26-675e44db3377"),
Guid.Parse("4afa543f-ffab-4909-b434-271e188751f3"),
Guid.Parse("3b7c0ac7-b775-4f97-816f-45848fa4c139"),
Guid.Parse("ebb90583-a6d2-4610-bdf1-ed67d89566ec"),
Guid.Parse("a6247136-a6fd-403b-b905-85a026fb49a1"),
Guid.Parse("5c589f15-e951-4df8-9608-b11e786ebef6"),
Guid.Parse("fac4c5ac-f15f-4969-bca4-1c88fb84b530"),
Guid.Parse("49e98e02-5856-43db-a6b0-53bf82affa41"),
Guid.Parse("d396f759-9dd7-4276-9f6b-c3708323e137"),
Guid.Parse("c62949a4-c532-4c2d-b435-cb75873ee2c0"),
Guid.Parse("92103fd1-294b-472e-ab0b-4f4e10dcd8e2"),
Guid.Parse("cb6288f1-fb48-4737-af63-170ea1ff094e"),
Guid.Parse("ddc991c9-9a92-41f3-9520-c9ea1733a478"),
Guid.Parse("86b593e2-5aa8-422e-81f3-cbb67d78dbbf"),
Guid.Parse("1e1a32d6-95cb-479c-ba13-f3a0bce2acf3"),
Guid.Parse("4a7bccb1-f5d7-4895-8e2e-40eaf179c2e9"),
Guid.Parse("ab635c1a-e9c9-4b5c-b6fb-b6b1ff6bca3d"),
Guid.Parse("266a6229-22ed-44c3-998d-f5c75acee9a2"),
Guid.Parse("ac79fa24-4f6c-49df-ab91-dc85d4fe7dd9"),
Guid.Parse("c7c78beb-0613-4b33-85eb-5717a634953f"),
Guid.Parse("703248b7-8872-4f37-8876-c7ffc47a4058"),
Guid.Parse("2b0af223-92b6-414f-b277-952420cb5a30"),
Guid.Parse("3e3d9436-32ff-46dc-b30b-c99e7c15b67c"),
Guid.Parse("a2c2a97c-29cf-4ca7-8df7-5faeef9d0901"),
Guid.Parse("cc2f60ff-2081-4269-b413-016e08d78434"),
Guid.Parse("4c14ed61-8f65-4fd6-94b8-f6059af18128"),
Guid.Parse("0319e55d-be82-42e3-9f13-fb49e2c57de3"),
Guid.Parse("f879a52a-5476-4091-b85b-79bee67e99f0"),
Guid.Parse("dc53da4f-2539-4831-bbcf-1884a468dd9d"),
Guid.Parse("67bb7809-4344-4432-8a25-d3f60e5df669"),
Guid.Parse("00993ee7-33b3-4c1d-99f6-0b1ff3fcf4d7"),
Guid.Parse("aebbcad9-aaa5-45a7-8813-30a25302272b"),
Guid.Parse("f7aa0c66-04ec-4beb-869d-46a671cff6aa"),
Guid.Parse("e1685e0c-b6ae-40bb-8440-9f4fd6371a4d"),
Guid.Parse("fb098730-f589-4294-9fcc-27fa0f4d1759"),
Guid.Parse("599313fb-314e-41aa-9f7a-af5eba7adde9"),
Guid.Parse("bc601f2a-7cc1-42a3-b0d7-e6bcd7a686c0"),
Guid.Parse("e5fca34a-2dca-42a9-b2cf-59d97d58cb94"),
Guid.Parse("2f431998-5ef0-4b60-81c0-d37c5affda65"),
Guid.Parse("ff01f480-5fab-4f07-bf8a-a6415bbedf2f"),
Guid.Parse("7d1a680d-68ae-4e05-b8db-408e70c5c2ed"),
Guid.Parse("b5b4b533-9285-4559-885d-c88da4167955"),
Guid.Parse("3a747b07-980d-4a26-9dcb-7278ce0bfb1b"),
Guid.Parse("1343e3af-1799-4840-aac7-f5defa2d2718"),
Guid.Parse("52fcba07-157b-4695-a342-84ceb8ecdd7e"),
Guid.Parse("92a6ccd1-7186-48d6-9849-95eb5fa3ef90"),
Guid.Parse("ca81f93e-36da-4929-ba97-9711c2188bb6"),
Guid.Parse("7823bd72-1bb5-4918-807a-7130b3232844"),
Guid.Parse("feadd422-547a-44e3-befc-22595d83b66a"),
Guid.Parse("d8fb0b54-38d5-4bed-8eba-540a775c35ca"),
Guid.Parse("77cc98ab-defe-4cec-9d0d-aecfbd812ca2"),
Guid.Parse("1c974619-d590-4d19-8942-6efc8f911f1f"),
Guid.Parse("0e484877-5658-4454-8600-f618652c74d1"),
Guid.Parse("38c410a5-5609-4205-aae3-5a6d2fd1ff4c"),
Guid.Parse("ca8f3158-876b-43b5-8e7d-e228396badb8"),
Guid.Parse("affbd8dc-6963-4506-b22c-cd4aa2b59ac6"),
Guid.Parse("086c1110-a61a-4194-9122-8896db28dd67"),
Guid.Parse("70362910-2642-4eec-ae06-58c301c33f2f"),
Guid.Parse("46663cc1-10bb-434a-a9b8-8bde16ace6a2"),
Guid.Parse("1f9f0903-9a04-4c71-aab9-cdc89b4e312d"),
Guid.Parse("35b1111d-1f39-44ee-8197-6f2a0476356f"),
Guid.Parse("41698cae-ffb9-4254-ad10-ef960ca93ebe"),
Guid.Parse("2f310b76-89b1-4f9f-9131-ad835c076f65"),
Guid.Parse("36e1e9c7-84e7-47af-b7ba-c36920a89c58"),
Guid.Parse("9998bc61-79ea-4906-b0ee-8f6ec95fe2fa"),
Guid.Parse("2e3f8c67-11ce-4b8f-bb8d-b7f85d1effdf"),
Guid.Parse("2cb4915f-aa43-4279-8af2-6839a1719912"),
Guid.Parse("cc9c3c86-d642-4de6-9d44-e5859af9fb42"),
Guid.Parse("d6b16085-548d-465a-9ff2-7fed5597aeac"),
Guid.Parse("67704614-0c40-46e8-b2f4-a48ffba95c7a"),
Guid.Parse("aec8d1b9-bf86-4d6c-9266-b635ecefc458"),
Guid.Parse("7f7c0c1f-5a57-4110-b355-6e274b0472d2"),
Guid.Parse("f4fcc538-8de4-4899-b06d-95b49e6f18d9"),
Guid.Parse("e79e31db-9afe-4842-b3bd-4420afcdda53"),
Guid.Parse("d624e07e-de82-4d48-8f74-f2b542d851d8"),
Guid.Parse("e23d69f8-da13-49cb-9f4d-6335fa9c5a89"),
Guid.Parse("d4095351-d2f7-446c-998d-bd67ddc984be"),
Guid.Parse("d37bbb0a-c3d1-4a89-8d7e-811a5831cc9c"),
Guid.Parse("5dcaa3b8-462f-421b-b1f2-dfdab14d461a"),
Guid.Parse("e9942d68-290d-4a50-a368-0c14dd282336"),
Guid.Parse("2308c5d9-83ce-46cd-ae83-3b8f12a1eb4b"),
Guid.Parse("90e97991-0ce8-4ba4-b412-5622c1da9a18"),
Guid.Parse("64f1717f-f738-4a4b-a2a7-1db3c893e51b"),
Guid.Parse("547c237a-738d-4495-a540-33f1ef4ded0b"),
Guid.Parse("64d15604-31c0-4d99-8dfa-23de75132fec"),
Guid.Parse("fe4f2234-57bd-4b26-b402-83d69dff8eae"),
Guid.Parse("7822d611-96b9-4341-a562-4253eb028941"),
Guid.Parse("60afe0f7-ec8f-4053-b7f7-1771e09e9f84"),
Guid.Parse("718b107d-3bd8-4531-9cb5-c0f352bc47fe"),
Guid.Parse("131a46f4-391f-4073-928e-15042da641d0"),
Guid.Parse("55acba1d-c754-4d7e-944a-9e218b0df77d"),
Guid.Parse("1214fcdb-aef0-4adb-8fa8-fc2c6a4c13e8"),
Guid.Parse("0075594c-81d2-4666-b952-ce3635eb1c5d"),
Guid.Parse("9f15f2aa-c245-4a84-b9eb-d48a09a6e42e"),
Guid.Parse("8c60ac72-8b4a-4712-b086-15c294db3a3a"),
Guid.Parse("a4b89046-d27a-416a-bd4b-0f7620ac0f7c"),
Guid.Parse("574dc9d9-291c-4d18-a12a-ed9224076a70"),
Guid.Parse("d0d9afb5-c478-433f-8d67-f551e38ec537"),
Guid.Parse("7214f438-bd82-4938-9c11-409a8d60d18c"),
Guid.Parse("29b1c009-a5e5-49bd-a64b-1b81cee34de8"),
Guid.Parse("68f9025c-53d7-4eb6-8ec9-7653638ec849"),
Guid.Parse("977c02f2-74bb-4231-a3db-f88f36a13b30"),
Guid.Parse("67eddf23-7f65-4e30-a0de-98868baffba1"),
Guid.Parse("147e48fa-4048-48b0-814c-be66b6fb80e1"),
Guid.Parse("5b856537-d54e-413f-a9b1-dfd17fddbe6d"),
Guid.Parse("8246b68c-da69-42a6-a42d-fba82de3bf0f"),
Guid.Parse("d8214e6c-8b96-488f-8eea-146d333a7af1"),
Guid.Parse("5307a741-2e6c-4583-bd66-d2535bb18cef"),
Guid.Parse("d12ed761-fd65-488d-a054-50838eeabe85"),
Guid.Parse("8acd76de-6fef-4f9d-87e3-54396b131f2e"),
Guid.Parse("33e8f632-d326-4c93-bdb1-02be9777873e"),
Guid.Parse("622ff1e5-83e2-4c26-88b6-788acf13c4b2"),
Guid.Parse("954d0db5-0cfd-458e-8908-0d0e004de38f"),
Guid.Parse("a90db712-bc80-47f8-92a8-c289ad99ce90"),
Guid.Parse("f0de0344-069f-4eef-a908-4fd1370275bb"),
Guid.Parse("427bb52b-095c-4750-982f-76aa87c811af"),
Guid.Parse("45fe228c-c172-42b4-a155-341ac0f82056"),
Guid.Parse("372f4780-679a-434e-8459-606d32fec60b"),
Guid.Parse("5ca70913-69e1-44c9-a75f-6408b3fc87d9"),
Guid.Parse("935f969b-dd87-40f8-b070-ee24860413b1"),
Guid.Parse("86262e49-dd98-4dc6-8071-ee6a36d7548f"),
Guid.Parse("abfc819b-cdcd-46de-bd18-7b528e8ebe60"),
Guid.Parse("8794c322-c0da-4f00-a12d-40e3c48c89bf"),
Guid.Parse("62fbc5d1-e62d-4ab5-a89d-b9e544825560"),
Guid.Parse("e49ca399-4437-4c4d-b5ac-1e847251719b"),
Guid.Parse("f36b017a-16c5-423d-92f9-e0123b06bc5c"),
Guid.Parse("a49446fa-e1e3-495a-89eb-d44bb3e93b50"),
Guid.Parse("6ea6509d-1092-4936-a8a8-a821183c0bf5"),
Guid.Parse("577bdbda-8111-45e1-a13d-5b7fd9704e46"),
Guid.Parse("8ab595da-a912-4119-a8b1-ebefeb63d204"),
Guid.Parse("2a85e4ab-61ed-4bf2-bf10-9bd5c03c1294"),
Guid.Parse("9a823386-2309-475c-b8b1-6c75f3d8c4e4"),
Guid.Parse("129192ad-e255-4a0a-8d8c-7288cf9e82fa"),
Guid.Parse("6f869593-9a0e-4309-b775-27c632e33ce6"),
Guid.Parse("30066682-cdcf-46e9-b0f6-a742cc3a12a8"),
Guid.Parse("01efcb43-02ca-4731-b691-74af1d453647"),
Guid.Parse("1a065e00-f77b-49e5-85e5-0e80ddeb1d91"),
Guid.Parse("79f2a7f9-677a-4c49-92a8-d88a8781e4ac"),
Guid.Parse("3d5b7071-e48f-41fa-adf3-956057722082"),
Guid.Parse("a9334ffd-6e49-4539-9753-78992a9fa386"),
Guid.Parse("e863901a-e0cf-4b6d-aa9d-4013d177e06b"),
Guid.Parse("880ec8b2-de35-4233-b540-bdeb86cf06f9"),
Guid.Parse("4e28f068-fba2-442f-a275-39bff4b0061b"),
Guid.Parse("6609fce5-34bf-4b01-9e04-05506b786d19"),
Guid.Parse("811403d4-5463-41e0-85bf-9dd078cadd09"),
Guid.Parse("3b7c04ec-6214-4652-9678-bff4b8cd2fe9"),
Guid.Parse("804d63fb-cd4c-4b64-9ec0-5aa6fadf7b61"),
Guid.Parse("5dd829b3-f2ec-40d6-abf9-6cddd0d6b505"),
Guid.Parse("db3efac4-5596-470b-9582-5aa2528bdee8"),
Guid.Parse("a2e54d14-39b1-4f10-8472-f0dac54e48f5"),
Guid.Parse("51bc217c-db68-4499-83bb-3c4b559c3728"),
Guid.Parse("74244e20-675e-44a4-a99b-1f755edb55f6"),
Guid.Parse("f5742b3f-b1be-4355-aec8-a5ac4be6a90e"),
Guid.Parse("a6ef0eb2-8f49-4f61-8987-e594f515b267"),
Guid.Parse("6280bb83-5843-4a1a-9617-329fa5a0cada"),
Guid.Parse("3882aec0-6d7f-4a20-ac6e-51682b52a808"),
Guid.Parse("10e217e9-c958-4cac-a691-baed3750acd4"),
Guid.Parse("54b1d33a-a5d7-4430-91dd-97179db4080a"),
Guid.Parse("860935d9-917d-4843-993a-3bebc8372052"),
Guid.Parse("5cc9600b-e282-489f-bf17-7f44bfdd0ae5"),
Guid.Parse("13a29d2a-a2c5-4a1f-89bc-57ae0ec42921"),
Guid.Parse("a51fc02f-ed1d-4eae-b8f1-7554300c3c8c"),
Guid.Parse("72bb0cd4-eef7-450c-90a3-ba6bb66a4c8a"),
Guid.Parse("4a9dcf28-9e6e-406d-a46c-54f6bef18988"),
Guid.Parse("154c26cd-99ad-4408-9ba9-98e256e487d9"),
Guid.Parse("39faf6c0-9b2b-40d5-a363-a6fe7b3e69ba"),
Guid.Parse("a4bcb950-730e-4313-a918-6018e01081a5"),
Guid.Parse("3a532f24-8fc9-42e8-a420-3693dc9033b4"),
Guid.Parse("da368cd9-502d-4d8f-9d7e-dab0db036709"),
Guid.Parse("f8f46262-cb81-45dd-9201-2f27ba2c3a28"),
Guid.Parse("7521ee68-3042-4101-8530-0765d3fb211e"),
Guid.Parse("421a6f21-2481-4192-8a72-1d6cfb2cf9d7"),
Guid.Parse("b7e14483-0087-4421-b990-53592d89280a"),
Guid.Parse("8aa682bd-833f-4ee9-999a-7e57c825c957"),
Guid.Parse("cbfcfa91-f296-4bee-a158-2bb5192fd616"),
Guid.Parse("be89588b-3a09-4af7-a553-3d224d817d09"),
Guid.Parse("de210432-46c1-4028-8b63-dcb37de86b0d"),
Guid.Parse("5dee8c9c-2572-490b-a70e-fc9216889476"),
Guid.Parse("e4fa4cfa-261b-4673-88cb-19dff6892cf9"),
Guid.Parse("a45fa8b0-7b3e-4a65-8dd3-7e94aea82132"),
Guid.Parse("5d400202-2768-43f0-9382-59a6337fd428"),
Guid.Parse("a669191e-e104-4172-a5dc-5b13c5dc24be"),
Guid.Parse("afc6e991-c012-4892-8b93-324942a9e9b3"),
Guid.Parse("381090f7-51ab-4d47-8f74-543c624561ab"),
Guid.Parse("68b1bf6f-3260-43ca-8e6b-a3725a700b1b"),
Guid.Parse("e2751781-eb55-4b60-ab0d-eaddd2b780ab"),
Guid.Parse("80e5fda3-3b00-4e58-ba92-93be28aca69e"),
Guid.Parse("6a294da4-7563-4518-9226-c06a7c9263f0"),
Guid.Parse("8c57d746-0e1c-49e8-aa8a-a8a6d679a55e"),
Guid.Parse("1809265a-f1d0-4848-b440-89ae6287def5"),
Guid.Parse("b741db83-1a69-4e7d-b05d-3a12aaba2c39"),
Guid.Parse("46c35bdf-d1db-4f6c-b892-1537411eccb0"),
Guid.Parse("d1e59575-31c3-4acd-bffc-fb4360e0018e"),
Guid.Parse("f7628faf-f3fe-4271-833f-3587e20ef19a"),
Guid.Parse("c8b1cf3d-7135-4ed6-b622-5e4429889952"),
Guid.Parse("6d88a834-4fda-4f5a-b95e-da28465044f2"),
Guid.Parse("16e3f0f4-65ba-4a3a-aded-9f37d668165b"),
Guid.Parse("ae61dd49-0d2d-4554-9e5a-4abf11d0e6ff"),
Guid.Parse("bbf13454-6326-44cf-a6f5-d2b6f11e600f"),
Guid.Parse("8d261f8b-8cdb-4e1c-847d-ee044649dd19"),
Guid.Parse("2a5a9d34-5ad2-40d2-9104-d76e6e1d4185"),
Guid.Parse("2bef8129-21d8-4a9c-89b4-50e2d70bc2ab"),
Guid.Parse("20f4e74e-9190-4525-af35-506030c83d63"),
Guid.Parse("900b82c0-def7-40a5-ac36-1a35485dfd7a"),
Guid.Parse("2fd4a8c3-56f8-4e0f-84d2-73741fd81fb0"),
Guid.Parse("aaa8023b-b37b-4022-94cf-439b8ff1a4e0"),
Guid.Parse("62bf4b33-75a2-4d3d-82c4-1074b37bc969"),
Guid.Parse("1d8cd4ec-22c6-4271-acac-c0b6478ae422"),
Guid.Parse("57dc665f-0352-4e39-aa9b-ea91c9a6874f"),
Guid.Parse("8ccb8265-f24f-49fb-8468-1ae00a7f6408"),
Guid.Parse("4b82c113-e170-4b61-9d86-d2224b610901"),
Guid.Parse("5977fea3-4c15-40de-9a4a-29d47a3548d8"),
Guid.Parse("6cba77e2-e555-4007-b369-d349efafef06"),
Guid.Parse("2eab3c8b-f893-4d2c-b77a-0eaf9bfe0212"),
Guid.Parse("5070cb3d-996b-4ced-bfbc-d710659ca732"),
Guid.Parse("62e23590-727e-4710-8c7f-6969c19c53ca"),
Guid.Parse("6613a481-4908-4486-ab1e-cc2e92758290"),
Guid.Parse("1eb574a0-30fe-4462-a1e8-3929852221db"),
Guid.Parse("9685353b-a5e3-433c-999a-135658e04a19"),
Guid.Parse("cea6e938-5028-4eeb-b5be-b0127f019a46"),

            };

            await identifiers.ParallelForEachAsync(async identifier =>
            {
                try
                {
                    var group = await this.groupsService.GetGroupAsync(identifier);
                    var zipModels = this.zipService.UnzipGroup(group.Content);
                    foreach (var document in group.Documents)
                    {
                        var zipModel = zipModels.First(z => z.Name == document.Name);
                        document.Content = zipModel.Content;
                    }

                    await this.ProcessGroupAsync(group);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Group was not process: {identifier};");
                }
            }, maxDegreeOfParallelism: 1);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process documents from converter: {crawlerName};");
        }

        this.Logger.LogInformation($"Converter: {crawlerName} end.");
    }

    protected HtmlDocument CreateHtmlDocument(Document document)
    {
        var encoding = Encoding.GetEncoding(document.Encoding);
        var html = encoding.GetString(document.Content).Decode();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument;
    }
}