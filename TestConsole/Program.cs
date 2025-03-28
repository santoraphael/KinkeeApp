using Miscellaneous.ELOCalulate;
using Mongo.BSN;
using Mongo.DAL;
using Mongo.Models;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Mongo.Services;

namespace TestConsole
{
    class Program
    {
        
        public static void Main(string[] args)
        {

            //QrCode qrCode = new QrCode();
            //QrCodeEstaticoModel qrCodeEstaticoModel = new QrCodeEstaticoModel();
            //qrCodeEstaticoModel.nome = "Raphael Santo";
            //qrCodeEstaticoModel.cidade = "São Paulo";
            //qrCodeEstaticoModel.chave = "33090854813";
            //qrCodeEstaticoModel.valor = 1.69;
            //qrCodeEstaticoModel.txid = "Kinkee-Pix-RAPHAELSANTO";
            //qrCodeEstaticoModel.saida = SaidaPix.br;

            //var teste = qrCode.RequestGeraQrCodeEstatico(qrCodeEstaticoModel);

            //Console.WriteLine(teste);
            //Console.ReadLine();

            //AfiliadosBSN afiliadosBSN = new AfiliadosBSN();
            //var produto = afiliadosBSN.GetProdutoByNome(Mongo.Models.Afiliados.NomesProdutos.AssinaturaDaddyPremiumMensal);

            //afiliadosBSN.GerarOperacaoOperacao("1265865", null, "5e0cc3db87c5a87d58e68be0", TipoOperacao.Assinatura, NomesTiposGanhos.PercentualCompras, new ObjectId("6111f475f39c6210b429be9b"),  new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao("0000000", null, null, TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, ,);

            //afiliadosBSN.GerarOperacaoOperacao(produto, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Assinatura, NomesTiposGanhos.PercentualCompras, NomesPerfis.semassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(produto, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Assinatura, NomesTiposGanhos.PercentualCompras, NomesPerfis.semassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(produto, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Assinatura, NomesTiposGanhos.PercentualCompras, NomesPerfis.semassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));


            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, NomesPerfis.comassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.InteracaoDaddy, NomesPerfis.influencer, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, NomesPerfis.comassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.InteracaoDaddy, NomesPerfis.influencer, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, NomesPerfis.comassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.InteracaoDaddy, NomesPerfis.influencer, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, NomesPerfis.comassinatura, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));
            //afiliadosBSN.GerarOperacaoOperacao(null, new ObjectId("5e0cc3db87c5a87d58e68be0"), TipoOperacao.Item, NomesTiposGanhos.InteracaoDaddy, NomesPerfis.influencer, new ObjectId("5e0cc3db87c5a87d58e68be0"), new ObjectId("5e0cc3db87c5a87d58e68be0"));

            //ComparaPerfis();



            //Mongo.Models.Compra.Item item = new Mongo.Models.Compra.Item();
            //item.title = "Pacote Kinkee Points 500";
            //item.unit_price = 1490;
            //item.quantity = 1;
            //item.tangible = false;

            //TransacaoDAL transacaoDAL = new TransacaoDAL();
            //transacaoDAL.InsertItem(item);


            //Random rnd = new Random();
            //int nrAcoesNoElo = 0;
            //int EloUsuario = 0;
            //int eloBase = rnd.Next(1, 2500);


            ELOVE.Acao AcaoPositivaNegativa = ELOVE.Acao.Positiva;
            int Player = 1;
            int SITE = 1;

            var calculado = ELOVE.CalculateELO(ref Player, false, ref SITE, ELOVE.TipoAcao.Basica, AcaoPositivaNegativa);

            Console.WriteLine(calculado);
            Console.ReadLine();
        }

        public static void ComparaPerfis()
        {
            AfiliadosBSN afiliadosBSN = new AfiliadosBSN();

            for (int i = 0; i <= 3; i++)
            {   
                var perfil = Mongo.Models.Afiliados.NomesPerfis.semassinatura;

                if (i == 0)
                {
                    perfil = Mongo.Models.Afiliados.NomesPerfis.semassinatura;
                }
                else if (i == 1)
                {
                    perfil = Mongo.Models.Afiliados.NomesPerfis.influencer;
                }
                else if (i == 2)
                {
                    perfil = Mongo.Models.Afiliados.NomesPerfis.comassinatura;
                }
                else
                {
                    break;
                }


                var produto = afiliadosBSN.GetProdutoByNome(Mongo.Models.Afiliados.NomesProdutos.AssinaturaDaddyPremiumMensal);

                var produtoCalculado = afiliadosBSN.CalculaValorLiquidoProduto(produto, perfil);
                var valorPremio = afiliadosBSN.CalcularPremioSobreValorProduto(produtoCalculado.ValorLiquido, perfil);
                var valorComissao = afiliadosBSN.CalcularComissaoSobreValorPremio(valorPremio, perfil);

                var valorRecebido = valorPremio - valorComissao;

                Console.WriteLine("Nome Produto: " + produtoCalculado.NomeProduto);
                Console.WriteLine("Valor Produto: " + produtoCalculado.ValorLiquido.ToString("C"));
                Console.WriteLine("Valor Bruto Premio Afiliado: " + valorPremio.ToString("C"));
                Console.WriteLine("Valor Comissao: " + valorComissao.ToString("C"));
                Console.WriteLine("Valor Recebido: " + valorRecebido.ToString("C"));


                Console.WriteLine("-----------------------------------");
            }
        }

        public async static void CadastrarLocation()
        {
            List<CountriesModel> listaPaises = ListaPaises().Result;
            List<StatesModel> listaEstados = null;
            List<CitiesModel> listaCidades = null;
            LocationBSN location = new LocationBSN();



            foreach (var pais in listaPaises)
            {

                Console.WriteLine(pais.Country_str_name);
                if(location.InsertCountry(pais))
                {
                    listaEstados = await ListaEstados(pais.Country_str_code);
                }
                else
                {
                    Console.WriteLine(pais.Country_str_name + ":   ERRRRRRRRRRRRRRRRRRRRRRRROOOOOO))))!!");
                    continue;
                }


                foreach (var estado in listaEstados)
                {
                    Console.WriteLine(estado.Admin1_str_name);
                    if(location.InsertState(estado))
                    {
                        listaCidades = await ListaCidades(estado.Admin1_str_code);
                    }
                    else
                    {
                        Console.WriteLine(estado.Admin1_str_name + ":   ERRRRRRRRRRRRRRRRRRRRRRRROOOOOO))))!!");
                        continue;
                    }
                   

                    foreach (var cidade in listaCidades)
                    {
                        Console.WriteLine(cidade.Feature_str_name);
                        if (location.InsertCity(cidade))
                        {

                        }
                        else
                        {
                            Console.WriteLine(cidade.Feature_str_name + ":   ERRRRRRRRRRRRRRRRRRRRRRRROOOOOO))))!!");
                            continue;
                        }
                    }
                }
            }
        }

        static async Task<List<CountriesModel>> ListaPaises()
        {
            var json_serializer = new JavaScriptSerializer();

            List<CountriesModel> ListPais = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://api.meupatrocinio.co");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/location/countries");
                
                if (response.IsSuccessStatusCode)
                {
                    //GET
                    ListPais = json_serializer.Deserialize<List<CountriesModel>>(response.Content.ReadAsStringAsync().Result);

                    
                }
            }

            return ListPais;
        }

        static async Task<List<StatesModel>> ListaEstados(string ID)
        {
            var json_serializer = new JavaScriptSerializer();

            List<StatesModel> ListEstados = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://api.meupatrocinio.co");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/location/"+ ID + "/states");

                if (response.IsSuccessStatusCode)
                {
                    //GET
                    ListEstados = json_serializer.Deserialize<List<StatesModel>>(response.Content.ReadAsStringAsync().Result);

                }
            }

            return ListEstados;
        }

        static async Task<List<CitiesModel>> ListaCidades(string ID)
        {
            var json_serializer = new JavaScriptSerializer();

            List<CitiesModel> ListCidades = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://api.meupatrocinio.co");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/location/" + ID + "/cities");

                if (response.IsSuccessStatusCode)
                {
                    //GET
                    ListCidades = json_serializer.Deserialize<List<CitiesModel>>(response.Content.ReadAsStringAsync().Result);

                }

                return ListCidades;
            }
        }
    }
}