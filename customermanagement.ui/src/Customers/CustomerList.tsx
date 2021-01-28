import React from "react";
import {
  withStyles,
  WithStyles,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Container,
  TableContainer,
  Paper,
  IconButton,
  Button,
} from "@material-ui/core";
import { Delete, Edit } from "@material-ui/icons";
import styles from "./Customer.styles";
import { ICustomer } from "../common/types/ICustomer";
import CustomerDetails from "./CustomerDetails";
import { CustomerMode, ViewMode } from "../common/types/enums";
import { FORM_ERROR } from "final-form";

interface IState {
  selectedCustomer?: ICustomer;
  editMode: CustomerMode;
}

interface IProps extends WithStyles<typeof styles> {
  customers: ICustomer[];
  viewMode: ViewMode;
  togglePopup: (showPopup: boolean) => void;
  showPopup: boolean;
  onDeleteCustomer: (customerId: number, viewMode: ViewMode) => Promise<void>;
  onAddCustomer: (customer: ICustomer, viewMode: ViewMode) => any;
  onEditCustomer: (customer: ICustomer, viewMode: ViewMode) => any;
  loadCustomers: (viewMode: ViewMode) => Promise<void>;
}

class CustomerList extends React.Component<IProps, IState> {
  readonly state: Readonly<IState> = {
    selectedCustomer: undefined,
    editMode: CustomerMode.Create,
  };

  private deleteCustomer = async (customerId: number, viewMode: ViewMode) => {
    await this.props.onDeleteCustomer(customerId, viewMode);
  };

  // showPopup() {
  //   this.setState({ showPopup: true });
  // }

  // closePopup = () => {
  //   this.setState({ showPopup: false });
  // };

  render() {
    const { customers, viewMode, togglePopup, showPopup, classes } = this.props;
    const { selectedCustomer } = this.state;

    return (
      <div>
        <Container>
          <Button
            variant="contained"
            color="primary"
            onClick={() => this.showDuplicateCustomer(viewMode)}
            className={classes.leftCornerButton}
          >
            {viewMode === ViewMode.AllCustomers
              ? "Resolve Conflicts"
              : "all Customers"}
          </Button>

          {viewMode === ViewMode.AllCustomers && (
            <Button
              variant="contained"
              color="primary"
              onClick={() => this.addCustomer()}
              className={classes.cornerButton}
            >
              Add Customer
            </Button>
          )}
          <br />

          <TableContainer component={Paper}>
            {customers && customers.length == 0 ? (
              viewMode === ViewMode.AllCustomers ? (
                "No Records Added"
              ) : (
                "No Conflicts to show"
              )
            ) : (
              <Table className={classes.summaryTable} size="small" stickyHeader>
                <TableHead className={classes.head}>
                  <TableRow className={classes.head}>
                    <TableCell>Name</TableCell>
                    {viewMode === ViewMode.AllCustomers && (
                      <TableCell>Email</TableCell>
                    )}
                    {viewMode === ViewMode.AllCustomers && (
                      <TableCell>Phone Number</TableCell>
                    )}
                    {viewMode === ViewMode.DuplicateCustomers && (
                      <TableCell>Duplicate Count</TableCell>
                    )}
                    <TableCell>Actions</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {customers?.map((customer, index) => {
                    return (
                      <React.Fragment key={index}>
                        <TableRow
                          hover
                          className={classes.menuRow}
                          style={
                            index % 2
                              ? { background: "rgba(0, 0, 0, 0.04)" }
                              : { background: "white" }
                          }
                        >
                          <TableCell>
                            <div>
                              {customer.firstName} {customer.lastName}
                            </div>
                          </TableCell>
                          {viewMode === ViewMode.AllCustomers && (
                            <TableCell>
                              <div>{customer.email}</div>
                            </TableCell>
                          )}

                          {viewMode === ViewMode.AllCustomers && (
                            <TableCell>
                              <div>{customer.phoneNumber}</div>
                            </TableCell>
                          )}

                          {viewMode === ViewMode.DuplicateCustomers && (
                            <TableCell>
                              <div>{customer.duplicateCount}</div>
                            </TableCell>
                          )}

                          <TableCell>
                            <IconButton
                              aria-label="edit"
                              onClick={() => this.editCustomer(customer)}
                            >
                              <Edit />
                            </IconButton>

                            {viewMode === ViewMode.AllCustomers && (
                              <IconButton
                                aria-label="remove"
                                onClick={() => {
                                  if (
                                    window.confirm(
                                      `Are you sure you want to delete ${customer.firstName} ${customer.lastName}`
                                    )
                                  ) {
                                    this.deleteCustomer(customer.id, viewMode);
                                  }
                                }}
                              >
                                <Delete />
                              </IconButton>
                            )}
                          </TableCell>
                        </TableRow>
                      </React.Fragment>
                    );
                  })}
                </TableBody>
              </Table>
            )}
          </TableContainer>
        </Container>

        {showPopup ? (
          <CustomerDetails
            customer={selectedCustomer}
            onClosePopup={togglePopup}
            onDelete={this.deleteCustomer}
            viewMode={viewMode}
            onSubmit={(customer: ICustomer) => {
              this.state.editMode === CustomerMode.Create
                ? this.props.onAddCustomer(customer, viewMode)
                : this.props.onEditCustomer(customer, viewMode);
            }}
          />
        ) : null}
      </div>
    );
  }

  editCustomer(customer: ICustomer): void {
    this.props.togglePopup(true);
    this.setState({
      selectedCustomer: customer,
      editMode: CustomerMode.Edit,
    });
  }

  addCustomer(): void {
    this.props.togglePopup(true);
    this.setState({
      selectedCustomer: undefined,
      editMode: CustomerMode.Create,
    });
  }

  showDuplicateCustomer(viewMode: ViewMode) {
    var newMode =
      viewMode === ViewMode.DuplicateCustomers
        ? ViewMode.AllCustomers
        : ViewMode.DuplicateCustomers;

    this.props.loadCustomers(newMode);
  }
}

export default withStyles(styles)(CustomerList);
