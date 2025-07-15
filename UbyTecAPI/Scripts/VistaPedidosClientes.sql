CREATE VIEW PedidosPorCliente AS
SELECT 
    p.PedidoID,
    p.Total AS MontoTotal,
    p.CantidadProducto,
    p.MontoProducto,
    p.NombreCliente,
    p.NombreComercio,
    p.NombreProducto
FROM Pedidos p
WHERE p.ClienteID IS NOT NULL;
