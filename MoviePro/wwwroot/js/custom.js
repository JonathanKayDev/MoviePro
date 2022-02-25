$(document).ready(function () {


    /*-------------------- Movie Import --------------------*/
    if (typeof query !== 'undefined') {
        // Confiugre Bloodhound
        let movieList = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace('name', 'value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: query,                         // Coming from the razor view
                wildcard: '%SEARCH',                // %SEARCH will be replaced by users input in the url option.
                transform: function (response) {    // This function will run before passing the results back to the page
                    let arr = response.results;
                    //Sort results by popularity
                    arr.sort((a, b) => {
                        return a.vote_average > b.vote_average ? -1 : 1;
                    });

                    let results = [];
                    arr.forEach(el => {
                        results.push({ name: el.original_title, value: el.id });
                    });

                    return results;
                }
            }
        });

        // Set typeahead for the input
        $('#movieSearch .typeahead').typeahead(
            {
                hint: true,
                highlight: true,
                minLength: 2,
            },
            {
                name: 'movie-list',
                source: movieList,                  // suggestion engine is passed as the source
                display: 'name',
                limit: 10,
                templates: {
                    pending: function (query) {
                        return '<div class="tt-suggestion">Loading...</div>';
                    }
                }
            }
        );

        $('.typeahead').on('typeahead:selected', function (evt, item) {
            // do what you want with the item here
            // set the hidden inputs value to the datum value eg. "21"
            $('#movie-id').val(item.value);
        })
    }

   
});