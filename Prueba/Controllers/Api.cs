using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;






namespace Lab02.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Api : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        //Nombre del archivo Original
        public static string ArchivOriginal;
        public static string UbicacionCom;
        public Api(IWebHostEnvironment environment)
        {
            _environment = environment;

        }
        [HttpPost("huffman/compress")]
        public IActionResult post([FromForm] IFormFile file)
        {

            try
            {
                if (file != null)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + file.FileName))
                    {
                        ArchivOriginal = file.FileName;
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        string temporal = file.FileName.ToString();
                        OperacionesH imp = new OperacionesH(fileStream.Name, s);
                        imp.Comprimir();

                        MemoryStream enviar = new MemoryStream(System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" +
                            Path.GetFileNameWithoutExtension(file.FileName) + ".huff"));

                        return File(enviar, "text/plain", Path.GetFileNameWithoutExtension(file.FileName) + ".huff");

                    }
                }
                else
                {
                    return StatusCode(500);
                }


            }
            catch (Exception e)
            {
                return StatusCode(500, new { name = "Internal Server Error", message = e.Message });
            }
        }

        [HttpPost("lzw/compress")]
        public IActionResult postLZW([FromForm] IFormFile file)
        {

            try
            {
                if (file != null)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + file.FileName))
                    {
                        ArchivOriginal = file.FileName;
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        string temporal = file.FileName.ToString();
                        OperacionesL imp = new OperacionesL(fileStream.Name, s);
                        imp.Comprimir();
                        MemoryStream enviar = new MemoryStream(System.IO.File.ReadAllBytes(_environment.WebRootPath +
                            "\\Upload\\" + Path.GetFileNameWithoutExtension(file.FileName) + ".lzw"));
                        return File(enviar, "text/plain", Path.GetFileNameWithoutExtension(file.FileName) + ".lzw");
                    }
                }
                else
                {
                    return StatusCode(500);
                }


            }
            catch (Exception e)
            {
                return StatusCode(500, new { name = "Internal Server Error", message = e.Message });
            }
        }

        [HttpGet("compressions")]
        public IEnumerable<Json> Get()
        {
            return Informacion.Instance.informacion.Select(x => new Json
            {
                nombreArchivo = ArchivOriginal,
                ubicacionComprimido = UbicacionCom,
                factorCompresion = x.factorCompresion,
                razonCompresion = x.razonCompresion,
                porcentajeCompresion = x.porcentajeCompresion
            });
        }



        [HttpPost("huffman/decompress")]
        public IActionResult decompress([FromForm] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + file.FileName))
                    {
                        ArchivOriginal = file.FileName;
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        string temporal = file.FileName.ToString();
                        OperacionesH imp = new OperacionesH(fileStream.Name, s);
                        imp.Descomprimir();
                        MemoryStream enviar = new MemoryStream(System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + Path.GetFileNameWithoutExtension(file.FileName) + ".txt"));
                        return File(enviar, "text/plain", Path.GetFileNameWithoutExtension(file.FileName) + ".txt");
                    }
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { name = "Internal Server Error", message = e.Message });
            }
        }

        [HttpPost("lzw/decompress")]
        public IActionResult decompressLzw([FromForm] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + file.FileName))
                    {
                        ArchivOriginal = file.FileName;
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        string temporal = file.FileName.ToString();
                        OperacionesL imp = new OperacionesL(fileStream.Name, s);
                        imp.Descomprimir();
                        MemoryStream enviar = new MemoryStream(System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + Path.GetFileNameWithoutExtension(file.FileName) + ".txt"));
                        return File(enviar, "text/plain", Path.GetFileNameWithoutExtension(file.FileName) + ".txt");
                    }
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { name = "Internal Server Error", message = e.Message });
            }
        }

    }
}
