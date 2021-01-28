import { createStyles } from '@material-ui/core/styles';

export default createStyles({
  head: {
    backgroundColor: "black",
    color: "white",
    textAlign: "center"
  },
  summaryTable: {
    minWidth: 650,
    whiteSpace: 'pre',

  },
  inputField: {
    width: '200px'
  },
  header: {
    marginBottom: '20px',
    backgroundColor: "#64b5f6 !important"
  },
  menuRow: {
    paddingLeft: 2,
    paddingRight: 1,
    height: 55,
    "&:hover": {
      backgroundColor: "#e3f2fd !important"
    }
  },


  tableHead: {
    backgroundColor: "#64b5f6 !important"
  },
  cornerButton: {
    float: 'right',
    margin: '15px'
  },
  leftCornerButton: {
    float: 'left',
    margin: '15px'
  },
  fieldsContainer: {
    //display: 'flex',
    justifyContent: 'space-evenly',
    alignItems: 'center',
    width: '250px',
    margin: 'auto',
    flexWrap: 'wrap',
    flexDirection:'row'
  },
  detailsPanel: {
    width: '25%',
    boxShadow: '0 0 5px 2px rgba(0, 0, 0, 0.3)',
    borderRadius: '5px',
    background: '#ffffff',
    position: 'relative',
  },
  singleLineElement: {
    flex: '1 0 25%',
    margin: '15px'
  }
});
