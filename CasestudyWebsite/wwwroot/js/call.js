$(function () {
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass();
        if ($("#CallModalForm").valid()) {
            $("#modalstatus").attr("class", "badge badge-success");
            $("#modalstatus").text("data entered is valid");

            $("#actionbutton").prop('disabled', false);
        }
        else {
            $("#modalstatus").attr("class", "badge badge-danger");
            $("#modalstatus").text("fix errors");

            $("#actionbutton").prop('disabled', true);
        }
    });

    $("#CallModalForm").validate({
        rules: {
            ddlPro: { required: true},
            ddlEmp: { required: true },
            ddlTech: { required: true },
            textNotes: { maxlength: 250, required: true }
        },
        errorElement: "div",
        messages: {
            ddlPro: {
                required: "Select Problem."
            },
            ddlEmp: {
                required: "Select Employee"
            },
            ddlTech: {
                required: "Select Tech."
            },
            textNotes: {
                required: "required 1-250 chars.", maxlength: "required 1-250 chars."
            },
        }
    });

    // retrieve the employee JSON from server and if retrieved successfully pass the data to the buildEmployeeList method
    const getAll = async (msg) => {
        try {

            // Load problem list
            let response = await fetch(`api/problem`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Problem server side, see server console`);
            let problems = await response.json();
            sessionStorage.setItem('allProblems', JSON.stringify(problems));

            // Load problem list
            response = await fetch(`api/employee`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Problem server side, see server console`);
            let employees = await response.json();
            sessionStorage.setItem('allEmployees', JSON.stringify(employees));

            $("#callList").text("Finding Call Information...");
            response = await fetch(`api/call/`); // FETCH API to make the REST call up to the server
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this retuen a promise, so we await it
            buildCallList(data, true);
            msg === "" ?  // are we appending to an existing message    
                $("#status").text("Calls Loaded") : $("#status").text(`${msg} - Calls Loaded`);

            let validator = $("#CallModalForm").validate();
            validator.resetForm();

        } catch (error) {
            $("status").text(error.message);
        }
    };// GetAll

    //empty the textboxes and sessionStorage fields
    const clearModalFields = () => {
        $("#textNotes").val("");
        $("#CallModalForm").validate().resetForm();
        //sessionStorage.removeItem("Id");
    }; // clearModalFields

    $("#callList").click((e) => {
        clearModalFields();
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === "callList" || Id === "") {
            Id = e.target.id;
        } // clicked on row somewhere else

        if (Id !== "status" && Id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allcalls"));
            Id === "0" ? setupForAdd() : setupForUpdate(Id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });  //employeeList

    const update = async () => {
        try {
            call = new Object(); // set up new client side instance of employee

            call.problemId = $("#ddlPro").val();
            call.employeeId = $("#ddlEmp").val();
            call.techId = $("#ddlTech").val();
            call.dateOpened = $("#dateOpen").val();
            call.notes = $("#textNotes").val();
            call.openStatus = $("#checkBoxClose").is(":checked") ? true : false;
            call.id = sessionStorage.getItem("Id");
            call.timer = sessionStorage.getItem("Timer");

            //var closeTime = new Date();
            //call.dateClosed = closeTime;
            call.dateClosed = sessionStorage.getItem("dateClosed");

            // send the updated back to the server asynchronously using PUT
            let response = await fetch("/api/call", {
                method: "PUT", // EmployeeController that will receive the updated student information
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call)
            });

            if (response.ok) {// or check for responese.status
                let data = await response.json();
                getAll(data.msg);
            }
            else {
                $("#status").text(`${response.status}, Text - ${response.statusText}`);
            }
            $("#theModal").modal("toggle");
        }
        catch (error) {
            $("#status").text(error.message);
        }
    };

    const add = async () => {
        try {
            call = new Object();

            call.problemId = $("#ddlPro").val();
            call.employeeId = $("#ddlEmp").val();
            call.techId = $("#ddlTech").val();
            call.dateOpened = $("#dateOpen").val();
            call.notes = $("#textNotes").val();

            call.id = -1;
            call.timer = null;

            // send the updated back to the server asynchronously using POST
            let response = await fetch("/api/call", {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call)
            });

            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            }
            else {
                $("#status").text(`${response.status}, Text - ${response.statusText}`);
            }
            $("#theModal").modal("toggle");
        }
        catch (error) {
            $("#status").text(error.message);
        }
    };

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "Update" ? update() : add();
    });

    const buildCallList = (data, usealldata) => {
        $("#callList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status"></div>
            <div class="list-group-item row d-flex text-center " id="heading">
                <div class="col-4 h4">Date</div>
                <div class="col-4 h4">For</div>
                <div class="col-4 h4">Problem</div>
            </div>`);
        div.appendTo($("#callList"));
        usealldata ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null;

        btn = $(`<button class="list-group-item row d-flex" id="0"><div class="col-12 text-left">...click to add call</div></button>`);
        btn.appendTo($("#callList"));

        

        data.map(call => {
           
            var dateStringWithTime = moment(call.dateOpened).format('YYYY-MM-DD HH:mm');
            let problemname = searchProblem(call.problemId);
            let empmname = searchEmpName(call.employeeId);
            call.employeeName = empmname;

            btn = $(`<button class="list-group-item row d-flex" id="${call.id}">`);
            btn.html(`  <div class="col-4" type="datetime-local" id="dateopen${call.dateOpened} "> ${dateStringWithTime} </div >
                        <div class="col-4" id="employee${call.employeeId} "> ${empmname}</div>
                        <div class="col-4" id="problem${call.problemId} "> ${problemname}</div>`);
            btn.appendTo($("#callList"));

            if (usealldata === true) {
                sessionStorage.removeItem("allcalls");
                sessionStorage.setItem("allcalls", JSON.stringify(data));
            }
        });
    };
    function searchProblem(id) {
        var pro = JSON.parse(sessionStorage.getItem('allProblems'));
        var desc = "";
        pro.map((e) => {
            if (e.id === id)
                desc = e.description;
        });

        return desc;
    }

    function searchEmpName(id) {
        var emp = JSON.parse(sessionStorage.getItem('allEmployees'));
        var name = "";
        emp.map(e => {
            if (e.id === id)
                name = e.firstname;
        });

        return name;
    }

    const setupForUpdate = (id, data) => {
        // show the delete button when doing an update
        $("#deletebutton").show();
        $("#divDateClose").show();
        $("#divCloseCall").show();

        $("#actionbutton").val("Update");
        $("#modaltitle").html("<h4>Update Call</h4>");

        clearModalFields();
        data.map(call => {
            if (call.id === parseInt(id)) {
                $("#textNotes").val(call.notes);

                sessionStorage.setItem("Id", call.id);
                sessionStorage.setItem("Timer", call.timer);

                $("#modalstatus").text("Update data");
                $("#theModal").modal("toggle");
                loadProblemDDL(call.problemId.toString());
                loadEmployeeDDL(call.employeeId.toString());
                loadTechnicianDDL(call.techId.toString());
                $("#dateOpen").val(formatDate(call.dateOpened));

                if (call.dateClosed !== null)
                    $("#dateClose").val(formatDate(call.dateClosed));

                if (call.openStatus) {
                    $("#ddlPro").attr('disabled', true);
                    $("#ddlEmp").attr('disabled', true);
                    $("#ddlTech").attr('disabled', true);
                    $("#checkBoxClose").attr('disabled', true);
                    $("#checkBoxClose").prop("checked", true);  
                    $("#textNotes").attr('readonly', true);
                    $("#actionbutton").hide();
                }
                else {
                    $("#ddlPro").attr('disabled', false);
                    $("#ddlEmp").attr('disabled', false);
                    $("#ddlTech").attr('disabled', false);
                    $("#checkBoxClose").attr('disabled', false);
                    $("#checkBoxClose").prop("checked", false);  
                    $("#textNotes").attr('readonly', false);
                    $("#actionbutton").show();
                    $("#dateClose").val("");
                }
            }
        });
    };

    const setupForAdd = () => {
        // hide the delete button when doing an add
        $("#ddlPro").attr('disabled', false);
        $("#ddlEmp").attr('disabled', false);
        $("#ddlTech").attr('disabled', false);
        $("#checkBoxClose").attr('disabled', false);
        $("#checkBoxClose").prop("checked", false);
        $("#textNotes").attr('readonly', false);
        $("#actionbutton").show();
        $("#dateClose").val("");

        $("#deletebutton").hide();
        $("#actionbutton").val("Add");
        $("#modaltitle").html("<h4>Add Call</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("Enter data");

        $("#divDateClose").hide();
        $("#divCloseCall").hide();

        //var now = new Date();
        //var dateStringWithTime = moment(now).format('YYYY-MM-DD HH:mm');
        $("#dateOpen").val(formatDate());

        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechnicianDDL(-1);
        clearModalFields();
    };
    // the confirmation click, passes a JSON object to the click event and the object contains the user’s choice. 
    // If the choice was yes, the delete button click is executed.If it’s no, the delete click button is ignored.
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
    $('#deletebutton').click(() => _delete());

    const _delete = async () => {
        try {
            let response = await fetch(`/api/call/${sessionStorage.getItem('Id')}`, {
                method: "DELETE",
                headers: { "Content-Type": "application/json; charset=utf-8" },
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`Status - ${response.status}, Problem on delete server side, see server console`);
            }
            $('#theModal').modal('toggle');
        }
        catch (error) {
            $('#status').text(error.message);
        }
    };

    // actually populate the html select with options
    const loadProblemDDL = (callPro) => {
        html = '';
        $('#ddlPro').empty();
        let allProblems = JSON.parse(sessionStorage.getItem('allProblems'));
        allProblems.map(c => html += `<option value = "${c.id}">${c.description}</option>`);
        $('#ddlPro').append(html);
        $('#ddlPro').val(callPro);
    };

    const loadEmployeeDDL = (callEmp) => {
        html = '';
        $('#ddlEmp').empty();
        let allEmployees = JSON.parse(sessionStorage.getItem('allEmployees'));
        allEmployees.map(e => html += `<option value = "${e.id}">${e.firstname}</option>`);
        $('#ddlEmp').append(html);
        $('#ddlEmp').val(callEmp);
    };

    const loadTechnicianDDL = (callTech) => {
        html = '';
        $('#ddlTech').empty();
        let allTechnician = JSON.parse(sessionStorage.getItem('allEmployees'));
        let filtereDdata = allTechnician.filter((empl) => empl.isTech == true);
        filtereDdata.map(e => html += `<option value = "${e.id}">${e.firstname}</option>`);
        $('#ddlTech').append(html);
        $('#ddlTech').val(callTech);
    };

    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allcalls"));
        let filtereddata = alldata.filter((call) => call.employeeName.match(new RegExp($("#srch").val(), 'i')));
        buildCallList(filtereddata, false);
    });

    $("#checkBoxClose").click(() => {
        if ($("#checkBoxClose").is(":checked")) {
            //var nowd = new Date();
            //var dateStringWithTimed = moment(nowd).format('YYYY-MM-DD HH:mm');
            $("#dateClose").val(formatDate());
            sessionStorage.setItem("dateClosed", formatDate());
        }
        else {
            $("#dateClose").val("");  
            sessionStorage.setItem("dateClosed", "");
        }
    });

    const formatDate = (date) => {
        let d;
        (date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
        let _day = d.getDate();
        let _month = d.getMonth() + 1;
        let _year = d.getFullYear();
        let _hour = d.getHours();
        let _min = d.getMinutes();

        if (_min < 10) { _min = "0" + _min; }

        return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    }

    getAll("");

});
