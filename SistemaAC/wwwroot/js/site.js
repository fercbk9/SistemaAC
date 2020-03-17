// Write your JavaScript code.
$('#ModalEditar').on('shown.bs.modal', function () {
  $('#myInput').trigger('focus')
})
$('#modalAgregarCategoria').on('shown.bs.modal', function () {
    $('#Nombre').trigger('focus')
})

$().ready(() => {
    document.getElementById("filtrar").focus();
    filtrarDatos(1,"nombre");
});

function getUsuario(id, action) {
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            mostrarUsuario(response);
        }
    });
}
var items;
var j = 0;

var id;
var userName;
var email;
var phoneNumber;
var role;
var selectRole;

var accessFailedCount;
var concurrencyStamp;
var emailConfirmed;
var lockoutEnabled;
var lockoutEnd;
var normalizedUserName;
var normalizedEmail;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;

function mostrarUsuario(response) {
    items = response; 
    j = 0;
    for (var i = 0; i < 3; i++) {
        var x = document.getElementById('Select');
        x.remove(i);
    }
    $.each(items, function (index, val) {
        $('input[name=Id]').val(val.id);
        $('input[name=UserName]').val(val.userName);
        $('input[name=Email]').val(val.email);
        $('input[name=PhoneNumber]').val(val.phoneNumber);
        document.getElementById('Select').options[0] = new Option(val.role, val.roleID);
        $("#dEmail").text(val.email);
        $("#dUserName").text(val.userName);
        $("#dRole").text(val.role);
        $("#dPhoneNumber").text(val.phoneNumber);
        $("#eUsuario").text(val.email);
        $("#eIdUsuario").val(val.id);
    });


}

function editarUsuario(action){
    id = $("input[name=Id]")[0].value;
    email = $("input[name=Email]")[0].value;
    phoneNumber = $("input[name=PhoneNumber]")[0].value;
    role = document.getElementById('Select');
    selectRole = role.options[role.selectedIndex].text;
    $.each(items, function (index, val) {
        accessFailedCount = val.accessFailedCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        normalizedUserName = val.normalizedUserName;
        normalizedEmail = val.normalizedEmail;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed= val.phoneNumberConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;
        userName = val.userName;
    });
    $.ajax({
        type: "POST",
        url: action,
        data: {
            id, userName, email, phoneNumber, accessFailedCount, concurrencyStamp, emailConfirmed, lockoutEnabled, lockoutEnd, normalizedEmail, normalizedUserName,
            passwordHash,phoneNumberConfirmed,securityStamp,twoFactorEnabled,selectRole
        },
        success: function (response) {
            if (response == "Save") {
                window.location.href = "Usuarios";
            } else {
                alert("No se pudo editar los datos del Usuario");
            }

        }
    });
}

function getRoles(action) {
    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j==0) {
                for (var i = 0; i < response.length; i++) {
                    document.getElementById('Select').options[i] = new Option(response[i].text, response[i].value);
                    document.getElementById('SelectNuevo').options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }
    });
}

function ocultarDetalleUsuario() {
    $('#modalDetalle').modal("hide");
}

function eliminarUsuario(action) {
    var id = $("input[name=eIdUsuario]")[0].value;
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            if (response==="Delete") {
                window.location.href = "Usuarios";
            } else {
                alert("No se puede eliminar el registro");
            }
        }
    });
}

function crearUsuario(action) {
    email = $('input[name=EmailNuevo]')[0].value;
    phoneNumber = $('input[name=PhoneNumberNuevo]')[0].value;
    passwordHash = $('input[name=PasswordHashNuevo]')[0].value;
    role = document.getElementById('SelectNuevo');
    selectRole = role.options[role.selectedIndex].text;
    var contador = 0;
    var text;
    var usuario = {
        'Email': email,
        'PhoneNumber': phoneNumber,
        'PasswordHash': passwordHash
    }
    if (email == '') {
        $('#EmailNuevo').focus();
        contador++;
        text = 'Rellene el campo Email.'
    }
    if (passwordHash == '') {
        $('#PasswordHashNuevo').focus();
        contador++;
        text = 'Rellene el campo Contraseña.'
    }
    switch (contador) {
        case 0:
            $.ajax({
                type: "POST",
                url: action,
                data: { selectRole,usuario },
                success: function (response) {
                    if (response) {
                        window.location.href = 'Usuarios';
                        limpiarCampos();
                    } else {
                        $('#mensajeNuevo').html("Error al dar de Alta el Usuario <br> La contraseña tiene que pasar de 6 caracteres con numeros, mayusculas y caracteres alfanumericos.");
                    }
                }
            });
            break;
        case 1:
            $('#mensajeNuevo').text(text);
            console.log(contador);
            break;
        case 2:
            $('#mensajeNuevo').text("Rellene los campos obligatorios");
            console.log(contador);
            break;
        
    }
    
}

function limpiarCampos() {
    $('input[name=EmailNuevo]').val("");
    $('input[name=PhoneNumberNuevo]').val("");
    $('input[name=PasswordHashNuevo]').val("");
    $("#mensajenuevo").html("");
    $('#SelectNuevo').children().remove().end().append('<option selected value="0">Seleccione un rol</option>');
    j = 0;
}

var idCategoria;
var agregarCategoria = () => {
    var nombre = document.getElementById("Nombre").value;
    var descripcion = document.getElementById("Descripcion").value;
    var estados = document.getElementById("Estado");
    var estado = estados.options[estados.selectedIndex].value;
    
    if (funcion == 0) {
        var action = 'Categorias/CreateCategoria';
        var categoria = new Categoria(nombre, descripcion, estado, action);
        categoria.agregarCategoria(categoria);
    } else {
        editarCategoria(estado);

    }
}

var filtrarDatos = (numPagina,order) => {
    var valor = document.getElementById("filtrar").value;
    var action = "Categorias/filtrarDatos";
    var categoria = new Categoria(valor, "", "", action);
    categoria.filtrarDatos(numPagina,order);
}
var funcion = 0;
var editarEstado = (id, fun) => {
    idCategoria = id
    funcion = fun
    var action = 'Categorias/getCategoria';
    var categoria = new Categoria("", "", "", action);
    categoria.getCategoria(id,funcion);
};

var editarCategoria = (estado) => {
    var action = 'Categorias/editarCategoria';
    if (estado == undefined) {
        estado = 0
        var categoria = new Categoria("", "", estado, action);
    } else {
        var nombre = document.getElementById('Nombre').value;
        var descripcion = document.getElementById('Descripcion').value
        var categoria = new Categoria(nombre, descripcion, estado, action);
    }
    
    categoria.editarCategoria(idCategoria, funcion);
}