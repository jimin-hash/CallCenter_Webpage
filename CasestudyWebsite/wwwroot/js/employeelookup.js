$(function () {
    $("#getbutton").click(async (e) => {
        try {
            let email = $("#TextBoxEmail").val();
            $("#status").text("please wait...");
            let response = await fetch(`api/employee/${email}`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Problem server side, see server console`);
            let data = await response.json(); // this retuen a promise, so we await it
            if (data.email !== "not found") {
                $("#lastname").text(data.lastname);
                $("#title").text(data.title);
                $("#firstname").text(data.firstname);
                $("#phone").text(data.phoneno);
                $("#status").text('employee found');
            }
            else {
                $("#firstname").text("not found");
                $("#email").text("");
                $("#title").text("");
                $("#phone").text("");
                $("#status").text("no such employee");
            }
        }
        catch (error) {
            $("#status").text(error.message);
        }
    });
});
