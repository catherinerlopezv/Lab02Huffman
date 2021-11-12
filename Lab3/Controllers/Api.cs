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
using LZW;





namespace Lab3.Controllers
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
                       
                        Jsonlzw.ManejarCompressions(
                           new Jsonlzw
                           {
                               NombreOriginal = ArchivOriginal,
                               Nombre = file.FileName,
                               RutaArchivo = Path.GetFullPath(file.FileName),
                               RazonCompresion = (double)file.Length / (double)ArchivOriginal.Length,
                               FactorCompresion = (double)ArchivOriginal.Length / (double)file.Length,
                               Porcentaje = 100 - (((double)file.Length / (double)ArchivOriginal.Length) * 100)
                           });
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

        [HttpGet("huffman/compressions")]
        public IEnumerable<Jsonlzw> Get()
        { 
       var compresiones = new List<Jsonlzw>();
        var logicaLIFO = new Stack<Jsonlzw>();
        var Linea = string.Empty;

            using (var Reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "compressions.json")))
            {
                while (!Reader.EndOfStream)
                {
                    var historialtemp = new Jsonlzw();
    Linea = Reader.ReadLine();
                    historialtemp.NombreOriginal = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.Nombre = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RutaArchivo = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RazonCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.FactorCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.Porcentaje = Convert.ToDouble(Linea);
                    logicaLIFO.Push(historialtemp);
                }
            }

            while (logicaLIFO.Count != 0)
{
    compresiones.Add(logicaLIFO.Pop());
}

return compresiones;
        }

        [HttpGet("lzw/compressions")]
        public IEnumerable<Jsonlzw> Getlzw()
        {
            var compresiones = new List<Jsonlzw>();
            var logicaLIFO = new Stack<Jsonlzw>();
            var Linea = string.Empty;

            using (var Reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "compressions.json")))
            {
                while (!Reader.EndOfStream)
                {
                    var historialtemp = new Jsonlzw();
                    Linea = Reader.ReadLine();
                    historialtemp.NombreOriginal = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.Nombre = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RutaArchivo = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RazonCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.FactorCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.Porcentaje = Convert.ToDouble(Linea);
                    logicaLIFO.Push(historialtemp);
                }
            }

            while (logicaLIFO.Count != 0)
            {
                compresiones.Add(logicaLIFO.Pop());
            }

            return compresiones;
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
