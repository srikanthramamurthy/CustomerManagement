import React from "react";
import { withStyles, WithStyles, IconButton, Button } from "@material-ui/core";
import Close from "@material-ui/icons/Close";
import styles from "./Customer.styles";
import { ICustomer } from "../common/types/ICustomer";
import { Form, Field } from "react-final-form";
import { ViewMode } from "../common/types/enums";
import { TextField } from "@material-ui/core";
import { FORM_ERROR } from "final-form";
import UnprocessableEntity from "../services/errors/UnprocessableEntity";

interface IState {
  showPopup: boolean;
}

interface IProps extends WithStyles<typeof styles> {
  customer?: ICustomer;
  viewMode: ViewMode;
  onClosePopup: (showPopup: boolean) => void;
  onDelete: (custerId: number, viewMode: ViewMode) => Promise<void>;
  onSubmit: (customer: ICustomer) => any;
}

const required = (value: any) => (value ? undefined : "Required");

class CustomerDetails extends React.Component<IProps, IState> {
  readonly state: Readonly<IState> = { showPopup: false };

  submitCustomer = async (customer: ICustomer) => {
    try {
      return await this.props.onSubmit(customer);
    } catch (ex) {
      if (ex instanceof UnprocessableEntity) {
        return {
          [FORM_ERROR]: ex.message,
        };
      }
    }
  };

  render() {
    const {
      customer,
      viewMode,
      onClosePopup,
      onDelete,
      onSubmit,
      classes,
    } = this.props;

    return (
      <div className="overlay">
        <div className={classes.detailsPanel}>
          <IconButton
            className={classes.cornerButton}
            onClick={() => onClosePopup(false)}
          >
            <Close />
          </IconButton>

          <Form
            onSubmit={this.submitCustomer}
            initialValues={customer}
            render={({ handleSubmit }) => (
              <form onSubmit={handleSubmit}>
                <div className={classes.fieldsContainer}>
                  <Field
                    name="firstName"
                    id="firstName"
                    type="text"
                    validate={required}
                  >
                    {({ input, meta }) => (
                      <div>
                        <TextField
                          {...input}
                          className={classes.singleLineElement}
                          placeholder="First Name"
                          error={meta.touched && meta.error}
                          helperText={
                            meta.error &&
                            meta.touched && (
                              <React.Fragment>{meta.error}</React.Fragment>
                            )
                          }
                        />
                      </div>
                    )}
                  </Field>

                  <Field name="lastName" id="lastName" validate={required}>
                    {({ input, meta }) => (
                      <div>
                        <TextField
                          {...input}
                          type="text"
                          className={classes.singleLineElement}
                          placeholder="Last Name"
                          error={meta.touched && meta.error}
                          helperText={
                            meta.error &&
                            meta.touched && (
                              <React.Fragment>{meta.error}</React.Fragment>
                            )
                          }
                        />
                      </div>
                    )}
                  </Field>

                  <Field name="email" id="email" validate={required}>
                    {({ input, meta }) => (
                      <div>
                        <TextField
                          {...input}
                          disabled={
                            viewMode == ViewMode.DuplicateCustomers
                              ? true
                              : false
                          }
                          type="email"
                          className={classes.singleLineElement}
                          placeholder="Email"
                          error={meta.touched && meta.error}
                          helperText={
                            meta.error &&
                            meta.touched && (
                              <React.Fragment>{meta.error}</React.Fragment>
                            )
                          }
                        />
                      </div>
                    )}
                  </Field>

                  <Field
                    name="phoneNumber"
                    id="phoneNumber"
                    validate={required}
                  >
                    {({ input, meta }) => (
                      <div>
                        <TextField
                          {...input}
                          type="text"
                          disabled={
                            viewMode == ViewMode.DuplicateCustomers
                              ? true
                              : false
                          }
                          className={classes.singleLineElement}
                          placeholder="Phone Number"
                          error={meta.touched && meta.error}
                          helperText={
                            meta.error &&
                            meta.touched && (
                              <React.Fragment>{meta.error}</React.Fragment>
                            )
                          }
                        />
                      </div>
                    )}
                  </Field>
                </div>

                <Button
                  variant="contained"
                  color="primary"
                  className={classes.cornerButton}
                  type="submit"
                >
                  Save
                </Button>

                {customer && (
                  <Button
                    variant="contained"
                    color="secondary"
                    className={classes.cornerButton}
                    type="button"
                    onClick={() => onDelete(customer.id, viewMode)}
                  >
                    Delete
                  </Button>
                )}
              </form>
            )}
          />
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(CustomerDetails);
