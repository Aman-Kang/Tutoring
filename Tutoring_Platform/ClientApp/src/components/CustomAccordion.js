import React, { Component } from 'react';
import Accordion from "@material-ui/core/Accordion";
import AccordionDetails from "@material-ui/core/AccordionDetails";
import AccordionSummary from "@material-ui/core/AccordionSummary";
export class CustomAccordion extends Component{
    render() {
        return (
            <div>
                <Accordion style={{ width: 800 }}>
                    <AccordionSummary
                        aria-controls="panel1a-content"
                    >
                        <h4>
                            {this.props.title}
                        </h4>

                    </AccordionSummary>
                    <AccordionDetails>
                        <div>{this.props.content}</div>
                    </AccordionDetails>
                </Accordion>
            </div>

        );
    }
}

