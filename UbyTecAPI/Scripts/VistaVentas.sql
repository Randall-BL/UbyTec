CREATE VIEW VentasGenerales AS
SELECT 
    p.PedidoID,
    p.ClienteID,
    p.AfiliadoID,
    p.Total AS MontoTotal,
    p.CantidadProducto,
    p.MontoProducto,
    p.NombreCliente,
    p.NombreComercio,
    p.NombreRepartidor
FROM Pedidos p;
