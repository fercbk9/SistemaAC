var localStorage = window.localStorage;
class Categoria {
    constructor(nombre, descripcion, estado, action) {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.estado = estado;
        this.action = action;
    }
    agregarCategoria(categoria) {
        if (this.nombre == "") {
            document.getElementById('Nombre').focus()
        } else {
            if (this.descripcion == "") {
                document.getElementById('Descripcion').focus()
            }
            else {
                if (this.estado == "0") {
                    document.getElementById('mensajeCat').innerHTML = "Seleccione Un Estado";
                } else {
                    var mensaje = '';
                    console.log(categoria);
                    $.ajax({
                        type: "POST",
                        url: this.action,
                        data: { categoria },
                        success: (response) => {
                            /*if (response) {
                                window.location.href = "Categorias";

                            } else {
                                document.getElementById('mensajeCat').innerHTML = "No se pudo Agregar";
                            }*/
                            $.each(response, (index, val) => {
                                mensaje = val.code;
                            });
                            if (mensaje == 'Save') {
                                this.restablecer()

                            } else {
                                document.getElementById('mensajeCat').innerHTML = "No se pudo agregar la categoria.";
                            }
                        }
                    })
                }
            }
        }
    }
    filtrarDatos(numPagina,order) {
        var valor = this.nombre;
        var action = this.action;
        if (valor == "") {
            valor = "null";
        }
        $.ajax({
            type: "POST",
            url: action,
            data: { valor, numPagina, order },
            success: (response) => {
                $.each(response, (index, val) => {
                    $("#resultSearch").html(val[0]);
                    $("#paginado").html(val[1]);

                });
            }
        });
    }
    getCategoria(id,funcion) {
        var action = this.action;
        $.ajax({
            type: "POST",
            url: action,
            data: { id },
            success: (response) => {
                if (funcion == 0) {
                    console.log(response);
                    if (response[0].estado) {
                        document.getElementById("titleEstado").innerHTML = "Está seguro de desactivar la categoria " + response[0].nombre + "?";
                    } else {
                        document.getElementById("titleEstado").innerHTML = "Desea activar la categoria " + response[0].nombre + "?";
                    }
                    localStorage.setItem("categoria", JSON.stringify(response));
                } else {
                    document.getElementById("Nombre").value = response[0].nombre;
                    document.getElementById("Descripcion").value = response[0].descripcion;
                    if (response[0].estado) {
                        document.getElementById("Estado").selectedIndex = 1;
                    } else {
                        document.getElementById("Estado").selectedIndex = 2;
                    }
                }
            }
        });
    }
    editarCategoria(id, funcion) {
        var c;
        switch (funcion) {
            case 0:
                var response = JSON.parse(localStorage.getItem("categoria"));
                c = {
                    CategoriaID: id,
                    Nombre: response[0].nombre,
                    Descripcion: response[0].descripcion,
                    Estado: response[0].estado
                };
                localStorage.removeItem("categoria");
                this.editar(c, funcion/*,id*/);
                break;
            default:
                if (this.nombre == "") {
                    document.getElementById('Nombre').focus()
                } else {
                    if (this.descripcion == "") {
                        document.getElementById('Descripcion').focus()
                    }
                    else {
                        if (this.estado == "0") {
                            document.getElementById('mensajeCat').innerHTML = "Seleccione Un Estado";
                        } else {
                            var c = {
                                CategoriaID: id,
                                Nombre: this.nombre,
                                Descripcion: this.descripcion,
                                Estado: this.estado
                            }
                            console.log(c);
                            this.editar(c, funcion);
                        }
                        break;
                    }
                }
        }
    }
    editar(categoria, funcion, id) {
        var action = this.action;
        var mensaje = '';
        $.ajax({
            type: "POST",
            url: action,
            data: { categoria, funcion/*,id*/ },
            success: (response) => {
                $.each(response, (index, val) => {
                    mensaje = val.code;
                });
                if (mensaje == 'Save') {
                    this.restablecer()

                } else {
                    document.getElementById('mensajeCat').innerHTML = "No se pudo agregar la categoria.";
                }
            }
        });

    }
    restablecer() {
        document.getElementById("Nombre").value = "";
        document.getElementById("Descripcion").value = "";
        document.getElementById("mensajeCat").value = "";
        document.getElementById("Estado").selectedIndex = 0;
        $("#modalAgregarCategoria").modal("hide");
        $("#ModalEstado").modal("hide");
        filtrarDatos(1,"nombre");
    }



}