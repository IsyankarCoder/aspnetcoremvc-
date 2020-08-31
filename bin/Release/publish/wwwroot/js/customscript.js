function confirmDelete(uniqueId,isDeleteClicked){
    var deleteSapan = 'deleteSpan_'+uniqueId;
    var confirmDeleteSpan  = 'confirmDeleteSpan_'+uniqueId;

    if(isDeleteClicked){
        $('#'+deleteSapan).hide();
        $('#'+confirmDeleteSpan).show();
    }
    else{
        $('#'+deleteSapan).show();
        $('#'+confirmDeleteSpan).hide();
    }
}