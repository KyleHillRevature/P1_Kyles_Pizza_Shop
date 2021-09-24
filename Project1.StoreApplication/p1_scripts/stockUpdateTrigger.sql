create trigger UpdateStock
on OrderItems
after insert
as
begin
declare @productID int
declare @locationID int
select @productID = inserted.ProductId, @locationID = LocationId
from inserted join LocationInventory
on inserted.ProductId = LocationInventory.ProductId

update LocationInventory
set Stock = Stock -1
where ProductId = @productID and LocationId = @locationID 

end
