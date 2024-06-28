// variables globales, limite de la tabla

var limitX = 10;
var limitY = 10;
// funcion para cargar correctamente el DOM
$(document).ready(function () {

    //funcion que se ejecuta posteriormente de cargar el DOM
    ready();
});
//add rows

function ready() {
    //se llaman las funciones
    console.log("ready!");
    let btnSend = document.getElementById("enviarFormulario").disabled = true;


    // funcion para pintar los bordes
    paintBorders();



    //funcion para obtener los data coord y cargar los data ady
    anRecorreAdy();
}
function anRecorreAdy() {

    //recorre cada item del td
    $.each($('table td'), function (i, item) {
        // se obtienen sus atributos y se parsea
        var data = $(item).attr('data-coord');
        var jsonData = JSON.parse(data);
        //console.log(jsonData);
        // a y b guardan el objeto x, y
        let a = jsonData.x;
        let b = jsonData.y;
        //console.log(a, b);
        //se ejecuta la funcion junto a los parametros (set)
        fnAdy(a, b);
        //guarda las variables que retorna la funcion (get)
        var dataAdy = fnAdy(a, b);

        dataAdy = JSON.stringify(dataAdy);
        // en cada item agrega en el atributo data-ady los datos de la variable dataAdy
        $(item).attr('data-ady', dataAdy);

    });

}
function fnAdy(x, y) {
    // adyascentes
    // arrayy vacio
    var ady = [];

    //Esquina izquierda arriba
    if ((x - 1) >= 0 && (y - 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x - 1, y: y - 1 })

    //Arriba
    if (x >= 0 && (y - 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x, y: y - 1 })

    //Esquina derecha arriba
    if ((x + 1) >= 0 && (y - 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x + 1, y: y - 1 })

    //izquierda
    if ((x - 1) >= 0 && y >= 0 && x < limitX && y < limitY)
        ady.push({ x: x - 1, y: y })

    //derecha
    if ((x + 1) >= 0 && y >= 0 && x < limitX && y < limitY)
        ady.push({ x: x + 1, y: y })

    //Esquina izquierda abajo
    if ((x - 1) >= 0 && (y + 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x - 1, y: y + 1 })

    //Abajo
    if (x >= 0 && (y + 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x, y: y + 1 })

    //Esquina derecha abajo
    if ((x + 1) >= 0 && (y + 1) >= 0 && x < limitX && y < limitY)
        ady.push({ x: x + 1, y: y + 1 })


    return ady;
}
function addRow() {

    // variable que marca el inicio de una tr
    var tr = "<tr>";
    // bucle que recorre las filas hasta el limite de columnas
    for (let i = 0; i < limitY; i++) {
        //mapea las coordenadas convierte el json en string
        let strDataCoord = JSON.stringify({ x: limitX, y: i });
        //concatena los atributos del html, en data coord se pasa el json parseado
        var concat = 'onclick="selectElm(this)" class="unsucces" data-ady="" data-coord=\'' + strDataCoord + '\'';
        // se le suman string a la variable tr, concatenando concat
        tr += "<td " + concat + "> X:" + limitX + " Y:" + i + "</td>";
    }
    //suma otro string que representa el cierre de la tr
    tr += "</tr>"
    //agrega tr como elemento hijo al elemento padre (table)
    $("table").append(tr);

    // se incrementa la variable limitX ya que representa el total de filas
    limitX = limitX + 1;
    debugger
    //ejecuta la funcion
    anRecorreAdy();
    paintBorders();

}
// add cols
function addCol() {

    // bucle for each que obtiene todos los elementos dentro del tr
    $.each($('table tr'), function (i, item) {
        //mapea las coordenadas convierte el json en string
        let strDataCoord = JSON.stringify({ x: i, y: limitY });
        //concatena los atributos del html, en data coord se pasa el json parseado
        var concat = 'onclick="selectElm(this)" class="unsucces" data-ady="" data-coord=\'' + strDataCoord + '\'';
        // en el elemento del tr se concatena la variable concat
        $(item).append("<td " + concat + ">X:" + i + " Y:" + limitY + "</td>");
    });


    // se incrementa la variable global
    limitY = limitY + 1;
    debugger;
    //se manda a llamar la funcion
    anRecorreAdy();
    // se ejecuta la funcion cada nuevo click para restablecer
    paintBorders();
}

function paintBorders() {

    $.each($('.tbl1j td'), function (i, item) {

        var data = $(item).attr('data-coord');
        var jsonData = JSON.parse(data);

        let a = jsonData.x;
        let b = jsonData.y;

        $(item).css('border-right', 'none');
        $(item).css('border-top', 'none');
        $(item).css('border-left', 'none');
        $(item).css('border-bottom', 'none');

        // arriba izquierda
        if (a == 0 && b == 0) {
            $(item).css('border-left', 'solid');
            $(item).css('border-top', 'solid');
        }

        //arriba derecha
        if (a == 0 && b == limitY - 1) {
            $(item).css('border-right', 'solid');
            $(item).css('border-top', 'solid');
        }

        //abajo izquierda
        if (a == limitX - 1 && b == 0) {
            $(item).css('border-left', 'solid');
            $(item).css('border-bottom', 'solid');
        }

        //abajo derecha
        if (a == limitX - 1 && b == limitY - 1) {
            $(item).css('border-right', 'solid');
            $(item).css('border-bottom', 'solid');
        }

        //lateral arriba
        if (a == 0 && b >= 1 && b < (limitY - 1)) {
            $(item).css('border-top', 'solid');
        }

        //lateral abajo
        if (a == (limitX - 1) && b >= 1 && b < (limitY - 1)) {
            $(item).css('border-bottom', 'solid');
        }

        //lateral derecha
        if (a >= 1 && a < (limitX - 1) && b == (limitY - 1)) {
            $(item).css('border-right', 'solid');
        }

        //lateral izquierda
        if (a >= 1 && a < (limitX - 1) && b == 0) {
            $(item).css('border-left', 'solid');
        }

    });

}
//function selectElm(elm) {
//    let eltd = $(elm).attr('class');
//    //const succes = document.querySelector('.succes');
//    //const unsucces = document.querySelector('.unsucces');
//    if (eltd!="succes"){
//         $(elm).attr('class', 'succes');
//        $(elm).css("background-color", "#d9edf7");
//        console.log("cambiar a succes");

//    }else if (eltd == "succes") {
//        var backgroundColor = $(elm).css("background-color");
//        console.log(backgroundColor);
//        $(elm).attr('class', 'unsucces');

//        if (backgroundColor == "rgb(217, 237, 247)") {
//            $(elm).css("background", "initial");

//            console.log("cambiar a unsucces");

//            $(elm).on('mouseover', function () {
//                $(this).css('background-color', 'grey');
//            });
//            var bgc = $(elm).css("background-color");
//            if (bgc != "rgb(217, 237, 247)") {
//                console.log(bgc);
//                //Restablecer el fondo al salir del hover
//                $(elm).on('mouseout', function () {
//                    $(this).css('background-color', 'white');
//                });
//            }

//        }

//    }

//}

//funcion para el hover y seleccion de item
var selectItem = [];
var selectAdya = [];
function selectElm(elm) {



    if (selectItem.length > 0) {
        //obtenemos coordenada seleccionada
        var coordSelect = $(elm).attr('data-coord');
        //parseamos la coordenada a un objeto JSON
        coordSelect = JSON.parse(coordSelect);
        //obtenemos las coordenadas adyacentes de la seleccionada
        var adyasSelect = $(elm).attr('data-ady');
        //parsear a json
        adyasSelect = JSON.parse(adyasSelect);
        var found = false;
        setTimeout(function () {
            //recorre cada item del td
            $.each($('table td'), function (i, item) {
                var dataAdysXY;
                let dataCoordsXY = $(item).attr('data-coord');
                dataCoordsXY = JSON.parse(dataCoordsXY);

                for (let j = 0; j < selectItem.length; j++) {

                    if (selectItem[j].x == dataCoordsXY.x && selectItem[j].y == dataCoordsXY.y) {
                        console.log("encontrado");
                        console.log(selectItem[j] + "" + dataCoordsXY);
                        // se obtienen los adyacentes
                        dataAdysXY = $(item).attr('data-ady');
                        dataAdysXY = JSON.parse(dataAdysXY);
                        //debugger

                        for (let f = 0; f < dataAdysXY.length; f++) {
                            if (coordSelect.x == dataAdysXY[f].x && coordSelect.y == dataAdysXY[f].y) {
                                console.log("encontrado coincidencia con adyacente");

                                selectItem.push(coordSelect);

                                elm.classList.remove('unsucces');

                                //agregamos la clase succes
                                elm.classList.add('succes');


                                console.log($(item).attr("class"));


                                console.log("todos los seleccionados");
                                console.log(selectItem);
                                found = true;
                                break; // Salir del bucle más interno
                            }

                        }
                    } else {
                        //console.log("error");
                    }
                    if (found) {
                        break; // Salir del bucle intermedio
                    }
                }

                if (found) {
                    return false; // Salir del bucle $.each
                }


            });
        }, 1);
        //debugger

    } else {
        //obtenemos coordenada seleccionada
        var coordSelect = $(elm).attr('data-coord');
        //parseamos la coordenada a un objeto JSON
        coordSelect = JSON.parse(coordSelect);
        //cargamos en el array
        selectItem.push(coordSelect);
        console.log("raiz iniciada");
        //borramos unsucces
        elm.classList.remove('unsucces');
        //agregamos la clase succes
        elm.classList.add('succes');
        console.log(selectItem);
        //let btn1 = document.getElementById("agrupar").disabled = false;
        let btn2 = document.getElementById("saveobject").disabled = false;
        let btn3 = document.getElementById("delete").disabled = false;

    }

    ////recorre cada item del td
    //$.each($('table td'), function (i, item) {
    //    // se obtienen sus atributos y se parsea
    //    let dataCoordsP = $(item).attr('data-coord');
    //    adyaCoords.push(dataCoordsP);

    //});
    //selectItem.push(coordSelect);
    //console.log("seleccionado");
    //console.log(coordSelect);
    //console.log("Adyacentes");
    //console.log(adyasSelect);
    //console.log(adyasSelect[1]);


    //si  la clase que contiene elm es diferente a succes
    //if (!elm.classList.contains('succes')) {
    //    //quitamos la clase unsucces
    //    elm.classList.remove('unsucces');
    //    //agregamos la clase succes
    //    elm.classList.add('succes');
    //    // elm.style.background = "#d9edf7";
    //    console.log("Cambiar a succes");
    //} else {
    //    //la clase succes se elimina
    //    elm.classList.remove('succes');
    //    //se agrega la clase unsucces
    //    elm.classList.add('unsucces');
    //   // elm.style.background = "initial";
    //    console.log("Cambiar a unsucces");
    //}
}
function deletebtn() {


    selectItem = []
    if (selectItem.length == 0) {
        console.log("delete");

        // Get a NodeList of all .demo elements
        const demoClasses = document.querySelectorAll('.succes');

        // Change the text of multiple elements with a loop
        demoClasses.forEach(element => {
            var elms = document.getElementsByClassName("succes");
            $(element).css('border-right', 'none');
            $(element).css('border-top', 'none');
            $(element).css('border-left', 'none');
            $(element).css('border-bottom', 'none');
            element.className = 'unsucces';
        });


    }
    document.getElementById("enviarFormulario").disabled = true;
    document.getElementById("saveobject").disabled = true;
}
function saveobject() {

    document.getElementById("enviarFormulario").disabled = false;
    // los items que tengan class succes aplicar estilos generales
    var elms = document.getElementsByClassName("succes");
    $(elms).css('border-right', 'solid');
    $(elms).css('border-top', 'solid');
    $(elms).css('border-left', 'solid');
    $(elms).css('border-bottom', 'solid');
    var i = 0;
    //primer bucle guarda una posicion del array selectitem
    while (i < selectItem.length) {
        //segundo bucle para comparar una posicion del selectitem con todas las demas posiciones y obtener su posicion de adyacencia
        for (let j = 0; j < selectItem.length; j++) {
            //debajo
            // debugger
            if ((selectItem[i].x + 1) == (selectItem[j].x) && selectItem[i].y == selectItem[j].y) {

                console.log(selectItem[j]);
                console.log("esta debajo de ");
                console.log(selectItem[i]);

                $.each($('table td'), function (f, item) {

                    let coord = $(item).attr('data-coord');
                    coord = JSON.parse(coord);
                    // debugger
                    if (coord.x == selectItem[i].x && coord.y == selectItem[i].y) {
                        console.log("encontrado en la vista! under");
                        console.log(coord);
                        $(item).css('border-bottom', 'none');

                    }
                });
            }
            //arriba
            debugger
            if ((selectItem[i].x - 1) == (selectItem[j].x) && selectItem[i].y == selectItem[j].y) {
                console.log(selectItem[j]);
                console.log("esta arriba de ");
                console.log(selectItem[i]);

                $.each($('table td'), function (f, item) {

                    let coord = $(item).attr('data-coord');
                    coord = JSON.parse(coord);
                    // debugger
                    if (coord.x == selectItem[i].x && coord.y == selectItem[i].y) {
                        console.log("encontrado en la vista! top");
                        console.log(coord);
                        $(item).css('border-top', 'none');


                    }

                });
            }
            //derecha
            debugger
            if ((selectItem[i].x) == (selectItem[j].x) && (selectItem[i].y + 1) == selectItem[j].y) {
                console.log(selectItem[j]);
                console.log("esta a la derecha de ");
                console.log(selectItem[i]);

                $.each($('table td'), function (f, item) {

                    let coord = $(item).attr('data-coord');
                    coord = JSON.parse(coord);
                    // debugger
                    if (coord.x == selectItem[i].x && coord.y == selectItem[i].y) {
                        console.log("encontrado en la vista! top");
                        console.log(coord);
                        $(item).css('border-right', 'none');
                    }

                });
            }
            //izquierda
            if ((selectItem[i].x) == (selectItem[j].x) && (selectItem[i].y - 1) == selectItem[j].y) {
                console.log(selectItem[j]);
                console.log("esta a la izquierda de ");
                console.log(selectItem[i]);
                //for each para encontrar la coordenada del primer bucle
                $.each($('table td'), function (f, item) {

                    let coord = $(item).attr('data-coord');
                    coord = JSON.parse(coord);
                    //debugger
                    if (coord.x == selectItem[i].x && coord.y == selectItem[i].y) {
                        console.log("encontrado en la vista! izq");
                        console.log(coord);
                        $(item).css('border-left', 'none');

                    }
                });

            }
        }
        i++;
    }
    $('#size_x').val(limitX);
    $('#size_y').val(limitY);
    var datosPARS = JSON.stringify(selectItem);
    $('#data_info').val(datosPARS);
    let btnSend = document.getElementById("enviarFormulario").disabled = false;
    let pisoS = document.getElementById('piso_snorkel').value;
    if (pisoS == null || pisoS == undefined || pisoS == "") {
        let btnSend = document.getElementById("enviarFormulario").disabled = true;
    } else {
        let btnSend = document.getElementById("enviarFormulario").disabled = false;
    }
    // var selectedOption = document.getElementById('id_area').value;

}

// 
$('#id_area').change(function () {
    var selectedOption = $(this).val();
    if (selectedOption == "2" || selectedOption == "3") {
        $('#id_subarea').prop('disabled', true);
    } else {
        $('#id_subarea').prop('disabled', false);
    }
});
//enviar POST
$('#formDiagram').submit(function (event) {
    event.preventDefault();

    var idSubareaSelect = $("#id_subarea");
    // Establecer el valor de id_subarea en null antes de enviar el formulario
    if (idSubareaSelect.prop("disabled")) {
        idSubareaSelect.val(null);
    }
    // Obtener los datos del formulario usando FormData
    var formData = new FormData(this);

    // Realizar la petición AJAX con los datos del formulario


    $.ajax({
        url: 'http://sagtdbw01/PlaceManagment/Tbl_Place_Assignment/JUAS/Tbl_Place_Diagram/Create',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            // Manejar la respuesta del servidor
            //console.log('Respuesta del servidor:', response);
            alert("Registrado!!");
            var url = 'http://sagtdbw01/PlaceManagment/Tbl_Place_Assignment/JUAS/Tbl_Place_Diagram/Index';
            window.location.href = url;
        },
        error: function (error) {
            // Manejar el error en caso de que ocurra
            console.error('Error en la petición:', error);
        }
    });
});
