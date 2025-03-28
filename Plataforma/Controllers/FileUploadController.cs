using ImageResizer;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class FileUploadController : Controller
    {
        UserBSN Usuario = new UserBSN();
        LogImageBSN _logImage = new LogImageBSN();

        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];

                //Suas validações ......

                //Salva o arquivo
                if (arquivo.ContentLength > 0)
                {
                    if (IsImage(arquivo))
                    {
                        var uploadPath = Server.MapPath("~/Content/Uploads");
                        string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));

                        arquivo.SaveAs(caminhoArquivo);
                        arquivosSalvos++;

                    }

                }
            }

            ViewData["Message"] = String.Format("{0} arquivo(s) salvo(s) com sucesso.",
                arquivosSalvos);
            return View("Upload");
        }

        [HttpPost]
        public ActionResult FileSobe(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {

                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

                        var instructions = new Instructions
                        {
                            Width = 800,
                            Height = 600,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                            instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        varUsuario.imagemPerfil = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);


                        //GERA UMA NOVA PUBLICACAO
                        DatingController _datingController = new DatingController();
                        _datingController.EnviarPublicacao("<img alt='' src=" + varUsuario.imagemPerfil + " style='width: 100 %; '>");

                        Usuario.EditarUsuario(varUsuario);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }


            return RedirectToAction("EditarPerfil", "MinhaConta");
        }


        [HttpPost]
        public ActionResult AtualizaFotoPerfil(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {

                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

                        //var information = Usuario.GetInformationByUserId(varUsuario.Id);

                        var instructions = new Instructions
                        {
                            Width = 800,
                            Height = 600,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                            instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        //if(information == null)
                        //{
                        //    information.UserInformationId = varUsuario.Id;
                        //}

                        //information.FotoParaAprovacao = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        varUsuario.imagemPerfilPrivado = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        //GERA UMA NOVA PUBLICACAO
                        //DatingController _datingController = new DatingController();
                        //_datingController.EnviarPublicacao("<img alt='' src=" + varUsuario.imagemPerfil + " style='width: 100 %; '>");

                        //Usuario.AlterUserInformation(information);

                        Usuario.EditarUsuario(varUsuario);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return RedirectToAction("Perfil", "Dating");
        }


        public ActionResult SalvaFotoGaleria(HttpPostedFileBase file)
        {

            var checkBox = Request.Form["isSelectedPrivate"];

            bool isPrivate = false;

            if (checkBox == "on")
            {
                isPrivate = true;
            }


            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {
                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        if (varUsuario.GaleriaFotos == null)
                        {
                            List<GaleriaFoto> galeriaFoto = new List<GaleriaFoto>();
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;
                            foto.isApproved = false;

                            galeriaFoto.Add(foto);

                            varUsuario.GaleriaFotos = galeriaFoto;

                        }
                        else
                        {
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;
                            foto.isApproved = false;

                            varUsuario.GaleriaFotos.Add(foto);
                        }

                        //SE NÂO FOR FOTO PRIVADA GERA UMA NOVA PUBLICACAO
                        if (!isPrivate)
                        {
                            DatingController _datingController = new DatingController();
                            _datingController.EnviarPublicacao("<img alt='' src=" + newVirtualPath + " style='width: 100 %; '>");
                        }

                        _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.Galery);

                        Usuario.EditarUsuario(varUsuario);
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return RedirectToAction("Perfil", "Dating");
        }


        public JsonResult CadastroSalvaFotoGaleria(HttpPostedFileBase file)
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var usuarioCriptografado = uri.PathAndQuery.Split('/').Last().Split('?').Last().Replace("user_id=", "");

            var checkBox = Request.Form["isSelectedPrivate"];

            bool isPrivate = false;

            if (checkBox == "on")
            {
                isPrivate = true;
            }


            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {
                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;

                        var nomeUsuario = SecurityHash.Descriptografar(usuarioCriptografado);

                        var varUsuario = UsuarioHelper.GetUsuario(nomeUsuario);


                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);
                        varUsuario.imagemPerfil = newVirtualPath;


                        if (varUsuario.GaleriaFotos == null)
                        {
                            List<GaleriaFoto> galeriaFoto = new List<GaleriaFoto>();
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;

                            galeriaFoto.Add(foto);

                            varUsuario.GaleriaFotos = galeriaFoto;


                        }
                        else
                        {
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;

                            varUsuario.GaleriaFotos.Add(foto);
                        }

                        
                        _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.Galery);

                        Usuario.EditarUsuario(varUsuario);
                    }

                }
                catch (Exception ex)
                {
                    var resultado = new
                    {
                        FotoSalva = false,
                        Retorno = "Não foi possível salvar sua foto. Verifique se a imagem não é muito grande e tente novamente.",
                        Erro = ex.Message.ToString()
                    };

                    return Json(resultado);
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }


            var resultadoOk = new
            {
                FotoSalva = true,
                Retorno = "Sua foto foi salva.",
                Erro = ""
            };

            return Json(resultadoOk);
        }

        [HttpPost]
        public string SalvaFotoGaleriaBase64(string file)
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var usuarioCriptografado = uri.PathAndQuery.Split('/').Last().Split('?').Last().Replace("user_id=", "");

            Byte[] bytes = Convert.FromBase64String(file);

            var instructions = new Instructions
            {
                Width = 1024,
                Height = 768,
                Mode = FitMode.Max,
                Format = "jpg",
                JpegQuality = 70,
            };

            var i = new ImageJob(bytes, "~/Users/<guid>_<guid>",
            instructions, false, true);
            i.CreateParentDirectory = true;
            i.Build();

            var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

            return newVirtualPath;
        }


        [HttpPost]
        public string SalvaImagemRetornaURL(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {

                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

                        //var information = Usuario.GetInformationByUserId(varUsuario.Id);

                        var instructions = new Instructions
                        {
                            Width = 800,
                            Height = 600,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                            instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        return ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return "";
        }


        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }


        public ActionResult ExcluirFotoGaleria(string idFoto)
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            varUsuario.GaleriaFotos.Remove(varUsuario.GaleriaFotos.Where(g => g.UrlFoto == idFoto).FirstOrDefault());

            Usuario.EditarUsuario(varUsuario);

            return RedirectToAction("Perfil", "Dating");
        }

        public ActionResult AlteraPrivacidadeFoto(string idFoto)
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);


            if (varUsuario.GaleriaFotos.Where(p => p.UrlFoto == idFoto).FirstOrDefault().isPrivate)
            {
                varUsuario.GaleriaFotos.Where(p => p.UrlFoto == idFoto).FirstOrDefault().isPrivate = false;
            }
            else
            {
                varUsuario.GaleriaFotos.Where(p => p.UrlFoto == idFoto).FirstOrDefault().isPrivate = true;
            }


            Usuario.EditarUsuario(varUsuario);

            return RedirectToAction("Perfil", "Dating");
        }
    }
}