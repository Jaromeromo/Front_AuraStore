import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { Request } from "../../api/requests";

type ProductoFormValues = {
  CodigoProducto: string;
  Nombre: string;
  Descripcion: string;
  VlrUnitario: number;
  VlrSinIva: number;
  VlrCompra: number;
  Stock: number;
  FechaIngreso: string;   // YYYY-MM-DD
  imagen: string | null;  // nombre de archivo o URL absoluta
};

export default function EditarProducto() {
  const { codigo } = useParams<{ codigo: string }>();
  const navigate = useNavigate();

  const [formData, setFormData] = useState<ProductoFormValues>({
    CodigoProducto: codigo?? "",
    Nombre: "",
    Descripcion: "",
    VlrUnitario: 0,
    VlrSinIva: 0,
    VlrCompra: 0,
    Stock: 0,
    FechaIngreso: "",
    imagen: null,
  });
  const [vistaPrevia, setVistaPrevia] = useState<string | null>(null);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const reqBase = useMemo(() => new Request("http://localhost:5000", {}), []);

  // 1) Cargar el producto por código
  useEffect(() => {
    (async () => {
      if (!codigo) { setError("Código no válido"); setCargando(false); return; }

      try {
        // Tu backend: GET /api/producto/ConsultarPorNit/{id}
        const resp = await reqBase.get(`producto/ConsultarPorNit/${encodeURIComponent(codigo)}`);

        // Si devuelve 404, resp.data será null → mantenemos el código y dejamos editar el resto
        if (!resp?.data) {
          setError("Producto no encontrado. Puedes editar y guardar de todas formas si el backend lo permite.");
        } else {
          const p = resp.data;
          setFormData(prev => ({
            ...prev,
            CodigoProducto: p.codigoProducto ?? p.CodigoProducto ?? prev.CodigoProducto,
            Nombre:         p.nombre ?? p.Nombre ?? "",
            Descripcion:    p.descripcion ?? p.Descripcion ?? "",
            VlrUnitario:    Number(p.vlrUnitario ?? p.VlrUnitario ?? 0),
            VlrSinIva:      Number(p.vlrSinIva ?? p.VlrSinIva ?? 0),
            VlrCompra:      Number(p.vlrCompra ?? p.VlrCompra ?? 0),
            Stock:          Number(p.stock ?? p.Stock ?? 0),
            FechaIngreso:   (p.fechaIngreso ?? p.FechaIngreso ?? "").slice(0,10),
            imagen:         p.imagen ?? null,
          }));
          setError(null);
        }
      } catch (e: any) {
        // 404 u otro error: no rompas el render, deja el código cargado
        setError(e.message || "No se pudo cargar el producto");
      } finally {
        setCargando(false);
      }
    })();
  }, [codigo, reqBase]);

  // URL de imagen actual (si viene nombre de archivo, la servimos desde el backend)
  const imagenActual = useMemo(() => {
    const img = formData.imagen;
    if (!img) return null;
    if (/^https?:\/\//i.test(img)) return img;
    return `http://localhost:5000/imagenes/productos/${img}`;
  }, [formData.imagen]);

  // 2) Cambios en inputs (incluye subida de imagen)
  const handleChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, files } = e.target;

    if (type === "file" && files?.[0]) {
      const file = files[0];
      setVistaPrevia(URL.createObjectURL(file));
      const fd = new FormData();
      fd.append("archivo", file);
      try {
        const resp = await axios.post("http://localhost:5000/api/Upload/subir/producto", fd, {
          headers: { "Content-Type": "multipart/form-data" },
        });
        // El backend devuelve el nombre del archivo
        setFormData(prev => ({ ...prev, imagen: resp.data }));
      } catch {
        setError("Error al subir la imagen");
      }
      return;
    }

    setFormData(prev => ({
      ...prev,
      [name]: type === "number" ? Number(value) : value,
    }));
  };

  // 3) Guardar cambios (PUT/POST según tu API)
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    try {
      // Si tu Request soporta PUT y tu API es RESTful:
      const reqUpdate = new Request("http://localhost:5000", formData);
      const resp = await reqUpdate.put(`producto/Actualizar/${codigo}`);

      // Si tu backend actualiza con POST, usa esta en su lugar:
      // const resp = await reqUpdate.post("producto/Actualizar");

      if (resp?.status >= 200 && resp?.status < 300) {
        navigate("/inventario");
      } else {
        throw new Error("Error al actualizar el producto");
      }
    } catch (e: any) {
      setError(e.message || "Error al actualizar");
    }
  };


  return (
    <>
    
    <div className="min-h-screen flex items-center justify-center bg-white p-25">
      <div className="bg-white p-8 rounded-xl shadow-xl w-full max-w-4xl border border-gray-200 ">
        <h1 className="text-center text-4xl font-bold text-purple-400 mb-8">Editar producto</h1>
        {error && <p className="text-red-500 text-center mb-4">{error}</p>}

        <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* Imagen */}
          <div className="flex flex-col items-center border border-purple-400 bg-gray-100 rounded-lg p-4 h-48 justify-center">
            <label htmlFor="imagen" className="cursor-pointer text-gray-500 text-sm text-center">
              {vistaPrevia ? (
                <img src={vistaPrevia} alt="Vista previa" className="h-full object-contain max-h-45" />
              ) : imagenActual ? (
                <img src={imagenActual} alt="Imagen actual" className="h-full object-contain max-h-45" />
              ) : (
                "Haz click para subir imagen"
              )}
              <input type="file" id="imagen" name="imagen" className="hidden" onChange={handleChange} />
            </label>
          </div>

          {/* Código: si es PK, lo dejamos solo lectura */}
          
          
          <div className="space-y-4">
          <Campo label="Código del producto" name="CodigoProducto" value={formData.CodigoProducto} onChange={handleChange} readOnly />
          <Campo label="Nombre" name="Nombre" value={formData.Nombre} onChange={handleChange} />
          </div>

          <Campo label="Cantidad en stock" name="Stock" type="number" value={String(formData.Stock)} onChange={handleChange} />
          <Campo label="Fecha ingreso" name="FechaIngreso" type="date" value={formData.FechaIngreso} onChange={handleChange} />
          <Campo label="Valor unitario COP" name="VlrUnitario" type="number" value={String(formData.VlrUnitario)} onChange={handleChange} />
          <Campo label="Valor sin IVA COP" name="VlrSinIva" type="number" value={String(formData.VlrSinIva)} onChange={handleChange} />
          <Campo label="Valor de compra COP" name="VlrCompra" type="number" value={String(formData.VlrCompra)} onChange={handleChange} />
          <Campo label="Descripción" name="Descripcion" value={formData.Descripcion} onChange={handleChange} />

          <div className="md:col-span-2 flex justify-end gap-4">
            <button type="button" onClick={() => navigate("/inventario")} className="bg-cyan-200 hover:bg-cyan-300 px-4 py-2 rounded-md">
              Cancelar
            </button>
            <button type="submit" className="bg-cyan-200 hover:bg-cyan-300 px-4 py-2 rounded-md">
              Guardar cambios
            </button>
          </div>
        </form>
      </div>  
    </div>
    </>
  );
}

/* Input simple reutilizable */
function Campo({
  label, name, value, onChange, type = "text", readOnly = false,
}: {
  label: string; name: string; value: string; type?: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void; readOnly?: boolean;
}) {
  return (
    <div>
      <label className="block text-sm font-medium">{label}</label>
      <input
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        readOnly={readOnly}
        className="mt-1 block w-full rounded-md bg-gray-100 p-2 focus:outline-none disabled:opacity-70"
      />
    </div>
  );
}
