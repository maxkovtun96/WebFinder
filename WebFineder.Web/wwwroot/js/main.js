(function () {
    function onFetchFindResponse(data) {
        let nodes = [];
        let edges = [];
        function recursive(node, prevNodeId) {
            let nodeId = nodes.length + 1;
            nodes.push({
                id: nodeId,
                label: `${node.siteUrl} (${node.wordsCount})`,
                value: node.wordsCount
            });
            if (nodeId > 1) edges.push({ from: prevNodeId, to: nodeId });
            $(node.subNodes).each(function (i, subNode) {
                recursive(subNode, nodeId);
            });
        }
        recursive(data, 0);
        let nodesDS = new vis.DataSet(nodes);
        let edgesDS = new vis.DataSet(edges);
        let container = document.getElementById('graph-container-id');
        let graphs = { nodes: nodesDS, edges: edgesDS };

        let network = new vis.Network(container, graphs, options);
    }

    function onSubmitHandler(e) {
        e.preventDefault();
        $("#graph-container-id").empty();
        let requiredParams = {
            url: $("#url-id").val(),
            word: $("#word-id").val()
        };
        $('#loader-id').show();
        $.get(findUrl, requiredParams, onFetchFindResponse)
            .done(function () {
                $('#loader-id').hide();
            });
    }

    function onDocReadyHandler() {
        $("#findWord-form-id").submit(onSubmitHandler);
    }

    $(document).ready(onDocReadyHandler);
})();

