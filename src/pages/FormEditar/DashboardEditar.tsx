import Sidebar from "../../components/pagInicio/sidebar";
import Header from "../../components/pagInicio/header";
import { useParams } from "react-router-dom";
import EditarProducto from "../../components/FormEditarProduct/EditProduct";

export default function DashboardEditar() {
  const { codigo } = useParams<{ codigo: string }>();

  return (
    <div className="flex w-full h-screen">
      <Sidebar />
      <main className="flex-1 bg-white p-6 overflow-auto">
        <Header />
        <EditarProducto/>
      </main>
    </div>
  );
}
