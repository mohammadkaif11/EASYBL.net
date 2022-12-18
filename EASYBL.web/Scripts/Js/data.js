function IncreamentPage() {
    var pageno = 0;
    address = window.location.search

    // Returns a URLSearchParams object instance
    parameterList = new URLSearchParams(address)


    // Storing every key value pair in the map
    parameterList.forEach((value) => {
        pageno = parseInt(value) + 1;
    })
        window.location.href = '/Bill/Data?page=' + pageno;
}

function DecrementValue() {
    var page = 0;
    address = window.location.search

    // Returns a URLSearchParams object instance
    parameterList = new URLSearchParams(address)


    // Storing every key value pair in the map
    parameterList.forEach((value) => {
        page = parseInt(value) -1;
    })
    if (page > 0) {
        window.location.href = '/Bill/Data?page=' + page;
    }
}