var list = []

// A $( document ).ready() block.
$( document ).ready(function() {
  var items = $(".img-container")
  list = [...items]
  AdjustOpacity(list)
  // right click
  $(".fa-arrow-right").on("click" , ()=>{
      first = list.shift()
      $(first).animate({
        "left" : "-=200px",
        "opacity" : 0,
      },1000, () => {
        $(first).css({
          "left" : "150px"
        })
        list.push($(first))
        AdjustOpacity(list)
      })
  })
  $(".fa-arrow-left").on("click" , ()=>{
    first = list.shift()
    $(first).animate({
      "left" : "+=200px",
      "opacity" : 0,
    },1000, () => {
      $(first).css({
        "left" : "150px"
      })
      list.push($(first))
      AdjustOpacity(list)
    })
})
});

function AdjustOpacity(items){
  if(items[0] != null || items[0] != undefined){
    $(items[0]).css({
      "opacity" : 1,
    });
  }
  for(var i = 1 ; i < Object.keys(items).length ; i++){
    $(items[i]).css({
      "opacity" : 0,
    });
  }
}