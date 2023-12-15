$(document).ready(function () {
    const $list = $(".book-row")

    $("#searchbutton").on("click", () => {
        const value_from_input = $("#searchinput").val().toLowerCase();

        $list.each(function () {
            var children = $(this).children();
            // Check if the search input matches any book title or author
            if (
                value_from_input === '' || // Show all books when the search input is empty
                value_from_input.includes(children.eq(2).text().toLowerCase()) ||
                value_from_input.includes(children.eq(3).text().toLowerCase())
            ) {
                $(this).fadeIn(); // Show the book
            } else {
                $(this).fadeOut(); // Hide the book
            }
        });
    });
});