CREATE VIEW VentasPorAfiliado AS
SELECT 
    p.PedidoID,
    p.ClienteID,
    p.Total AS MontoTotal,
    p.CantidadProducto,
    p.MontoProducto,
    p.NombreComercio,
    p.NombreCliente,
    p.NombreRepartidor
FROM Pedidos p
WHERE p.AfiliadoID IS NOT NULL;
