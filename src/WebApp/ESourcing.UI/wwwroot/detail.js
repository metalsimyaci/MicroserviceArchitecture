import { signalR } from "../../../lib/microsoft-signalr/signalr";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:16526/auctionhub");
var auctionId = document.getElementById("AuctionId").value;

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.start().then(function() {
	document.getElementById("sendButton").disabled = false;
	connection.invoke("AddToGroup", groupName).catch(function(err) {
		return console.error(err.toString());
	});
}).catch(function(err) {
	return console.error(err.toString());
});

connection.on("Bids", function (user, bid) {
	addBidToTable(user, bid);
});


document.getElementById("sendButton").addEventListener("click", function (event) {

	const user = document.getElementById("SellerUserName").value;
	const productId = document.getElementById("ProductId").value;
	const sellerUser = user;
	const bid = document.getElementById("exampleInputPrice").value;

	const sendBidRequest = {
		AuctionId: auctionId,
		ProductId: productId,
		SellerUserName: sellerUser,
		Price: parseFloat(bid).toString()
	};

	SendBid(sendBidRequest);
	event.preventDefault();
});

document.getElementById("finishButton").addEventListener("click", function (event) {

	const sendCompleteBidRequest = {
		AuctionId: auctionId,
	};
	SendCompleteBid(sendCompleteBidRequest);
	event.preventDefault();
});



function addBidToTable(user, bid) {
	var str = "<tr>";
	str += "<td>" + user + "</td>";
	str += "<td>" + bid + "</td>";
	str += "</tr>";

	if ($('table > tbody> tr:first').length > 0) {
		$('table > tbody> tr:first').before(str);
	} else {
		$('.bidLine').append(str);
	}

}

function SendBid(model) {
	$.ajax({
		url: "/Auction/SendBid",
		type: "POST",
		data: model,
		success: function (response) {
			if (response.isSuccess) {
				document.getElementById("exampleInputPrice").value = "";
				connection.invoke("SendBidAsync", groupName, model.SellerUserName, model.Price).catch(function (err) {
					return console.error(err.toString());
				});
			}
		},
		error: function (jqXHR, textStatus, errorThrown) {
			console.log(textStatus, errorThrown);
		}
	});
}

function SendCompleteBid(model) {
	const id = auctionId;
	$.ajax({

		url: "/Auction/CompleteBid",
		type: "POST",
		data: { id: id },
		success: function (response) {
			if (response) {
				console.log("ıslemınız basarıyla sonuclandı");
				location.href = "https://localhost:44398/Auction/Index";
			}
		},
		error: function (jqXHR, textStatus, errorThrown) {
			console.log(textStatus, errorThrown);
		}
	});
}