$(function () {
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass();
        if ($("#EmployeeModalForm").valid()) {
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

    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            }
        }
    });

    $.validator.addMethod("validTitle", (value) => {
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, "");

    // retrieve the employee JSON from server and if retrieved successfully pass the data to the buildEmployeeList method
    const getAll = async (msg) => {
        try {
            
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee/`); // FETCH API to make the REST call up to the server
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this retuen a promise, so we await it
            buildEmployeeList(data, true);
            msg === "" ?  // are we appending to an existing message    
                $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);

            // Load department list
            response = await fetch(`api/department`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status = ${response.status}, Problem server side, see server console`);
            let deps = await response.json();
            sessionStorage.setItem('alldepartments', JSON.stringify(deps));

            let validator = $("#EmployeeModalForm").validate();
            validator.resetForm();

        } catch (error) {
            $("status").text(error.message);
        }
    };// GetAll

    //empty the textboxes and sessionStorage fields
    const clearModalFields = () => {
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("Id");
        sessionStorage.removeItem("DepartmentId");
        sessionStorage.removeItem("Timer");
        sessionStorage.removeItem("Picture");

        $("#ImageHolder").html(`<img height="120" width="110" src="" />`);
        //$("#uploader").val("");

        $("#EmployeeModalForm").validate().resetForm();
    }; // clearModalFields

    $("#employeeList").click((e) => {
        clearModalFields();
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === "employeeList" || Id === "") {
            Id = e.target.id;
        } // clicked on row somewhere else

        if (Id !== "status" && Id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees"));
            Id === "0" ? setupForAdd() : setupForUpdate(Id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });  //employeeList

    const update = async () => {
        try {
            emp = new Object(); // set up new client side instance of employee

            // populate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();

            emp.id = sessionStorage.getItem("Id");
            emp.departmentId = $("#ddlDes").val();

            emp.timer = sessionStorage.getItem("Timer");
            //emp.Picture64 = null;

            sessionStorage.getItem("Picture")
                ? emp.picture64 = sessionStorage.getItem("Picture")
                : null;

            // send the updated back to the server asynchronously using PUT
            let response = await fetch("/api/employee", {
                method: "PUT", // EmployeeController that will receive the updated student information
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp)
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
            emp = new Object();

            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            emp.departmentId = $("#ddlDes").val();

            emp.id = -1;
            emp.timer = null;
            //emp.Picture64 = null;

            sessionStorage.getItem("Picture")
                ? emp.picture64 = sessionStorage.getItem("Picture")
                : null;

            // send the updated back to the server asynchronously using POST
            let response = await fetch("/api/employee", {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp)
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

    const buildEmployeeList = (data, usealldata) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Sample BootStrap 4 Styling </div>
            <div class="list-group-item row d-flex text-center " id="heading">
                <div class="col-4 h4">Title</div>
                <div class="col-4 h4">First</div>
                <div class="col-4 h4">Last</div>
            </div>`);
        div.appendTo($("#employeeList"));
        usealldata ? sessionStorage.setItem("allemployees", JSON.stringify(data)) : null;

        btn = $(`<button class="list-group-item row d-flex" id="0"><div class="col-12 text-left">...click to add employee</div></button>`);
        btn.appendTo($("#employeeList"));

        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`  <div class="col-4" id="employeetitle${emp.id} "> ${emp.title} </div >
                        <div class="col-4" id="employeefname${emp.id} "> ${emp.firstname}</div>
                        <div class="col-4" id="employeelastname${emp.id} "> ${emp.lastname}</div>`);
            btn.appendTo($("#employeeList"));
        });
    };

    const setupForUpdate = (id, data) => {
        // show the delete button when doing an update
        $("#deletebutton").show();

        $("#actionbutton").val("Update");
        $("#modaltitle").html("<h4>Update Employee</h4>");

        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstname").val(employee.firstname);
                $("#TextBoxLastname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);

                $("#ImageHolder").html(`<img height="120" width="110" src="data:image/png;base64,${employee.picture64}" />`);

                sessionStorage.setItem("Id", employee.id);
                sessionStorage.setItem("DepartmentId", employee.departmentId);
                sessionStorage.setItem("Timer", employee.timer);
                sessionStorage.setItem("Picture", employee.picture64);

                $("#modalstatus").text("Update data");
                loadDepartmentDDL(employee.departmentId.toString());
                $("#theModal").modal("toggle");
            }
        });
    };

    const setupForAdd = () => {
        // hide the delete button when doing an add
        $("#deletebutton").hide();

        $("#actionbutton").val("Add");
        $("#modaltitle").html("<h4>Add Employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("Add new Employee");
        loadDepartmentDDL(-1);
        clearModalFields();
    };
    // the confirmation click, passes a JSON object to the click event and the object contains the user’s choice. 
    // If the choice was yes, the delete button click is executed.If it’s no, the delete click button is ignored.
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
    $('#deletebutton').click(() => _delete());

    const _delete = async () => {
        try {
            let response = await fetch(`/api/employee/${sessionStorage.getItem('Id')}`, {
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
    const loadDepartmentDDL = (empdep) => {
        html = '';
        $('#ddlDes').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.map(dep => html += `<option value = "${dep.id}">${dep.name}</option>`);
        $('#ddlDes').append(html);
        $('#ddlDes').val(empdep);
    };

    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allemployees"));
        let filtereddata = alldata.filter((emp) => emp.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildEmployeeList(filtereddata, false);
    });

    $("input:file").change(() => {
        const reader = new FileReader();
        const file = $("#uploader")[0].files[0];

        file ? reader.readAsBinaryString(file) : null;

        reader.onload = (readerEvt) => {
            // get binary data then convert to encoded string
            const binaryString = reader.result;
            const eccodeString = btoa(binaryString);
            sessionStorage.setItem("Picture", eccodeString);
        }
    });

    getAll("");

});
