// components/pagInventario/InventarioCard.tsx
import { FaEdit } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

export type InventoryCardProps = {
  codigoProducto: string;          // <-- agrega esto
  imagen: string;
  nombre: string;
  descripcion: string;
  stock: number;
};

const InventarioCard = ({ codigoProducto, imagen, nombre, descripcion, stock }: InventoryCardProps) => {
  const navigate = useNavigate();

  return (
    <div className="flex bg-yellow-50 border-2 border-yellow-300 rounded-xl p-4 shadow-md w-[650px] h-[200px] items-center justify-between hover:shadow-lg transition">
      {/* Imagen */}
      <div className="w-32 h-32 flex-shrink-0">
        <img src={`http://localhost:5000/imagenes/productos/${imagen}`} alt={nombre} className="w-full h-full object-contain" />
      </div>

      {/* Info */}
      <div className="flex-1 px-6 text-left">
        <p className="font-bold text-gray-800 mb-1">Stock: {stock}</p>
        <h2 className="text-lg font-semibold text-gray-700 mb-1">{nombre}</h2>
        <p className="text-sm text-gray-600">{descripcion}</p>
      </div>

      {/* Botón editar (el de la captura) */}
      <div className="flex flex-col items-center gap-4">
        <button
          type="button"
          onClick={() => navigate(`/inventario/edit/${codigoProducto}`)}   // <-- aquí navega
          className="bg-cyan-200 hover:bg-cyan-300 text-cyan-800 p-2 rounded-full"
          aria-label="Editar producto"
          title="Editar producto"
        >
          <FaEdit size={18} />
        </button>
      </div>
    </div>
  );
};

export default InventarioCard;
