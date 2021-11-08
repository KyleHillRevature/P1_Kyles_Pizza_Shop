create trigger IncreaseStock
on OrderItems
after delete
as
begin
declare @productID int
declare @locationID int
select @productID = ProductId, @locationID = LocationId
from deleted join Orders
on deleted.OrderId = Orders.Id

update LocationInventory
set Stock = Stock + 1
where ProductId = @productID and LocationId = @locationID 

end
