var options = {
    nodes: {
        shape: "dot",
        scaling: {
            customScalingFunction: function (min, max, total, value) {
                return value / total;
            },
            min: 5,
            max: 50
        }
    },
    edges: {
        arrows: {
            to: {
                enabled: true,
                scaleFactor: 1
            }
        }
    }
};