using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using POOI_EF_PEÑA.Models;

namespace POOI_EF_PEÑA.Controllers
{
    public class ConsumidorController : Controller
    {
       
        private readonly string cadena =
            ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        // Metodos
        List<Consumidor> ListarConsumidores()
        {
            var lista = new List<Consumidor>();
            using (var cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (var cmd = new SqlCommand("usp_listar_consumidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Consumidor
                            {
                                dnicont = dr.GetString(0),
                                nomcont = dr.GetString(1),
                                apecont = dr.GetString(2),
                                emailcont = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                idpais = dr.IsDBNull(4) ? "" : dr.GetString(4)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        string AgregarConsumidor(Consumidor c)
        {
            string mensaje;
            using (var cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (var cmd = new SqlCommand("usp_agrega_consumidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", (object)c.dnicont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nomcont", (object)c.nomcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@apecont", (object)c.apecont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailcont", (object)c.emailcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@idpais", (object)c.idpais ?? DBNull.Value);

                    var pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMensaje.Value);
                }
            }
            return mensaje;
        }

        string EliminarConsumidor(string dni)
        {
            string mensaje;
            using (var cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (var cmd = new SqlCommand("usp_eliminar_consumidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", dni);

                    var pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMensaje.Value);
                }
            }
            return mensaje;
        }


        Consumidor BuscarConsumidor(string dni)
        {
            Consumidor c = null;
            using (var cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (var cmd = new SqlCommand("usp_buscar_consumidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", dni);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            c = new Consumidor
                            {
                                dnicont = dr.GetString(0),
                                nomcont = dr.GetString(1),
                                apecont = dr.GetString(2),
                                emailcont = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                idpais = dr.IsDBNull(4) ? "" : dr.GetString(4)
                            };
                        }
                    }
                }
            }
            return c;
        }

        string ActualizarConsumidor(Consumidor c)
        {
            string mensaje;
            using (var cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (var cmd = new SqlCommand("usp_actualiza_consumidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", (object)c.dnicont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nomcont", (object)c.nomcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@apecont", (object)c.apecont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailcont", (object)c.emailcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@idpais", (object)c.idpais ?? DBNull.Value);

                    var pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMensaje.Value);
                }
            }
            return mensaje;
        }

        public ActionResult Index()
        {
            ViewBag.mensaje = TempData["mensaje"];
            return View(ListarConsumidores());
        }

        public ActionResult Create()
        {
            return View(new Consumidor());
        }

        [HttpPost]
        public ActionResult Create(Consumidor c)
        {
            ViewBag.mensaje = AgregarConsumidor(c);
            return View(c);
        }

        public ActionResult Delete(string id)
        {
            TempData["mensaje"] = EliminarConsumidor(id);
            return RedirectToAction("Index");
        }

        public ActionResult Update(string id)
        {
            Consumidor c = BuscarConsumidor(id);
            if (c == null)
            {
                TempData["mensaje"] = "Error: no se encontró el consumidor";
                return RedirectToAction("Index");
            }
            return View(c);
        }

        [HttpPost]
        public ActionResult Update(Consumidor c)
        {
            ViewBag.mensaje = ActualizarConsumidor(c);
            return View(c);
        }

        public ActionResult Details(string id)
        {
            Consumidor c = BuscarConsumidor(id);
            if (c == null)
            {
                ViewBag.mensaje = "No se encontró el consumidor con DNI " + id;
                return View(new Consumidor());
            }
            ViewBag.mensaje = "Consumidor encontrado correctamente";
            return View(c);
        }
    }
}